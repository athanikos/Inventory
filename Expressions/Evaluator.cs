
using MediatR;
using Inventory.Products.Contracts;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Inventory.Expressions;
using Serilog;
using Inventory.Notifications.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Expressions
{
    public class Evaluator : IEvaluator
    {
        private const string OpenBracket = "[";
        private const string ClosingBracket = "]";
        private const char Comma = ',';
        private const string OpenParenthesis = "(";
        private const string SUM = "SUM";
        private char[] _operators = ['*', '/', '+', '-', '>', '<' ];
        private string[] aggregateFunctions = ["SUM", "AVG"];
        private const string TRUE = "TRUE";
        private string ALLSpecifier = "[ALL]";
        private string _expression = string.Empty;
        private readonly IMediator _mediator;
        private readonly ExpressionsDbContext _context;
        private List<string> _allProductCodes;
        private List<string> _allMetricCodes;
        private Guid _inventoryId;

        public Evaluator(IMediator mediator, ExpressionsDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<EvaluatorResult> Execute(Guid inventoryId, string expression)
        {
            _expression = expression;
            _inventoryId = inventoryId;
            GetCodes();
            return await ComputeTokens(ParseTokens());
        }

        private async void GetCodes()
        {
            var response = await _mediator.Send(new CodesQuery(_inventoryId));
            if (response == null)
                throw new ArgumentNullException();

            _allProductCodes = response.ProductCodes;
            _allMetricCodes = response.MetricCodes;
        }

        private async Task<EvaluatorResult> ComputeTokens(List<string> tokens)
        {
            var resultedExpression = string.Empty;
            foreach (var token in tokens)
            {
                if (IsNumeric(token) || IsOperator(token) )
                    resultedExpression += token.ToString().Trim();
                else if (IsInventoryBasedFormula(token))
                {
                    var er = (await ComputeComplexFunction(token,
                                                 ExtractAggregateFunction(token)));
                    resultedExpression += er.Result.ToString().Trim();
                }
                else if (IsProductBasedFormula(token))
                {

                    var er2 = (await ComputeSimpleFunction(_inventoryId, token));
                    resultedExpression += er2.Result.ToString().Trim();
                }
            }

            if (string.IsNullOrEmpty(resultedExpression))
                return EvaluatorResult.NewUndefinedResult();


            try
            {

                Log.Information("NCalc.Evaluate:" + resultedExpression);
                var value = new NCalc.Expression(resultedExpression).Evaluate().ToString();
                return EvaluatorResult.NewEvaluatorResult(value.ToString());

            }
            catch (Exception ex)
            {
                
                Log.Error(ex.ToString());
                return EvaluatorResult.NewUndefinedResult();
            }

        }
        /// <summary>
        /// breaks expression into list of strings 
        /// </summary>
        /// <returns></returns>
        private List<string> ParseTokens()
        {
            List<string> resultedList = new List<string>();
            string currentToken = string.Empty;

            for (int i = 0; i < _expression.Length; i++)
            {
                if (_operators.Contains(_expression[i]))
                {
                    if (currentToken != string.Empty)
                        resultedList.Add(currentToken);
                    currentToken = string.Empty;

                    resultedList.Add(_expression[i].ToString());
                }
                else
                    currentToken += _expression[i].ToString();
            }

            if (currentToken != string.Empty)
                resultedList.Add(currentToken);

            return resultedList;
        }



        private string ExtractAggregateFunction(string token)
        {
            int indexOfParenthesis = token.IndexOf(OpenParenthesis);
            return token.Substring(0, indexOfParenthesis);
        }

        // todo: per inventoryId 

        /// <summary>
        ///     Parses a formula  of the following  
        ///     SUM( VALUE(FUNC(ALL),Latest))
        ///     SUM( VALUE([ADA,XRP],Latest))
        ///     one prod one metric 
        ///     returns the value in product metric table 
        /// </summary>
        /// <param name="token"></param>
        private async Task<EvaluatorResult> ComputeComplexFunction(string token,
            string aggregateFunction)
        {
            DateTime upperboundDate = DateTime.MaxValue;
            List<string> productCodes = null;
            string metricCode = string.Empty;
          
            try
            {
                metricCode = ExtractMetricCode(token);
                productCodes = ExtractProductCodes(token);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return EvaluatorResult.NewUndefinedResult();
            }

            string result = string.Empty;
            foreach (var productCode in productCodes)
            {
                if (aggregateFunction == SUM)
                {
                    try
                    {
                        var dto = (await _mediator.Send(
                                   new GetProductMetricQuery(_inventoryId, productCode, metricCode, upperboundDate)));
                        result += result != string.Empty ? "+" + dto.Value : string.Empty + dto.Value;
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        return EvaluatorResult.NewUndefinedResult();
                    }
                }
                else
                    throw new ArgumentException("aggregate function not supported: " + aggregateFunction);

            }
            return EvaluatorResult.NewEvaluatorResult(result);
        }

        private string ExtractMetricCode(string token)
        {
            foreach (var item in _allMetricCodes)
                if (token.Contains(item))
                    return item;

            throw new ArgumentException(token);
        }

        private List<string> ExtractProductCodes(string token)
        {
            List<string> items = new List<string>();
            if (token.Contains(ALLSpecifier))
                items.AddRange(_allProductCodes);
            else
                items.AddRange(ExtractProducts(token).Split(Comma));
            return items;
        }

        private static string ExtractProducts(string token)
        {
            int startIndexOfProducts = token.IndexOf(OpenBracket) + 1;
            int endIndexOfProducts = token.IndexOf(ClosingBracket) - 1;
            var length = endIndexOfProducts - startIndexOfProducts;
            string products = token.Substring(startIndexOfProducts, length + 1);
            return products;
        }


        /// <summary>
        ///     Parses a formula  of the following   Quantity(Ada,Latest)
        ///     one prod one metric 
        ///     returns the value in product metric table 
        /// </summary>
        /// <param name="token"></param>
        private async Task<EvaluatorResult> ComputeSimpleFunction(Guid InventoryId, string token)
        {
            string productCode = string.Empty;
            string metricCode = string.Empty;
            DateTime upperboundDate = DateTime.Now;

            foreach (var item in _allProductCodes)
                if (token.Contains(item))
                    productCode = item;

            foreach (var item in _allMetricCodes)
                if (token.Contains(item))
                    metricCode = item;
            try
            {
                string result =  (await _mediator.
                      Send(new GetProductMetricQuery(InventoryId, productCode, metricCode, upperboundDate))).Value.ToString();
              

                return EvaluatorResult.NewEvaluatorResult(result);

            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return EvaluatorResult.NewUndefinedResult();

            }
        }

        private expressionType _type = expressionType.undefined;

        public enum expressionType
        {
            undefined = -1,
            productBased = 0,
            inventoryBased = 1
        }

        private bool IsProductBasedFormula(string token)
        {

            if (IsInventoryBasedFormula(token))
                _type = expressionType.inventoryBased;

            foreach (var item in _allProductCodes)
                if (token.Contains(item))
                    _type = expressionType.productBased;

            foreach (var item in _allMetricCodes)
                if (token.Contains(item))
                    _type = expressionType.productBased;

            return expressionType.productBased == _type;
        }


        private bool IsOperator(string token)
        {
            return token.Length == 1 && _operators.Contains(token[0]);
        }

        private bool IsNumeric(string token)
        {
            return decimal.TryParse(token, out var result);
        }

        /// <summary>
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsInventoryBasedFormula(string token)
        {
            foreach (var item in aggregateFunctions)
                if (token.Contains(item))
                {
                    _type = expressionType.inventoryBased;
                    return expressionType.inventoryBased == _type;
                }

            if (token.Contains(ALLSpecifier))
            {
                _type = expressionType.inventoryBased;
                return expressionType.inventoryBased == _type;
            }
            var numberOfProducts = 0;
            foreach (var item in _allProductCodes)
                if (token.Contains(item))
                    numberOfProducts++;
            if (numberOfProducts > 1 )
                _type = expressionType.inventoryBased;


            return expressionType.inventoryBased == _type;
        }

        public List<Entities.ProductExpression> GetProductExpressions()
        {
            return [.. _context.ProductExpressions];
        }

        public List<Entities.InventoryExpression> GetInventoryExpressions()
        {
            return [.. _context.InventoryExpressions];
        }

        public List<Entities.BooleanExpression> GetBooleanExpressions()
        {
            return [.. _context.BooleanExpressions];
        }

        public void ScheduleJobs(IServiceProvider serviceProvider)
        {
            var productExpressions = GetProductExpressions();
            foreach (var p in productExpressions)
                RecurringJob.AddOrUpdate(p.Id.ToString(), () => DoScheduledWork(p), Cron.Minutely);

            var inventoryExpressions = GetInventoryExpressions();
            foreach (var i in inventoryExpressions)
                RecurringJob.AddOrUpdate(i.Id.ToString(), () => DoScheduledWork(i), Cron.Minutely);

            var booleanExpressions = GetBooleanExpressions();
            foreach (var i in booleanExpressions)
                RecurringJob.AddOrUpdate(i.Id.ToString(), () => DoScheduledWork(i), Cron.Minutely);


        }



        public  void  DoScheduledWork(Entities.InventoryExpression ie)
        {
            try
            {
                DoScheduledWorkAsync(ie).Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message,ex);
            }
        }

        public  void DoScheduledWork(Entities.ProductExpression pe)
        {
            try
            {
                DoScheduledWorkAsync(pe).Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }

        }

        public void DoScheduledWork(Entities.BooleanExpression be)
        {
            try
            {
                DoScheduledWorkAsync(be).Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }

        }

        public async Task DoScheduledWorkAsync(Entities.InventoryExpression p)
        {


            var result = await Execute(p.TargetInventoryId, p.Expression);

            if (_type == expressionType.undefined)
                throw new ArgumentException(_type.ToString());

            if (_type == expressionType.inventoryBased)
            {

                Log.Information("AddInventoryMetricCommand");

                var command = new AddInventoryMetricCommand(p.TargetInventoryId,
                                                    p.TargetMetricId,
                                                   decimal.Parse(result.Result),
                                                    DateTime.Now, "EUR"); //todo get from product 

                Log.Information("_mediator.Send(command)");

                await _mediator.Send(command);


            }

        }

        public async Task DoScheduledWorkAsync(Entities.ProductExpression p)
        {
            var result = await Execute(p.InventoryId, p.Expression);

            if (_type == expressionType.undefined)
                throw new ArgumentException(_type.ToString());

            if (result.Type == EvaluatorResult.EvaluatorResultType.undefined)
                return;

            if (_type == expressionType.productBased)
            {
                Log.Information("AddProductMetricCommand");



                var command = new AddProductMetricCommand(p.TargetProductId,
                                                          p.TargetMetricId,
                                                          decimal.Parse(result.Result),
                                                          DateTime.Now, "EUR"); //todo get from product 
                await _mediator.Send(command);
            }
        }

        public async Task DoScheduledWorkAsync(Entities.BooleanExpression p)
        {

            Log.Information("DoScheduledWorkAsync with BooleanExpression");
            var result = await Execute(p.InventoryId, p.Expression);

            if (_type == expressionType.undefined)
                throw new ArgumentException(_type.ToString());

            if (result.Type == EvaluatorResult.EvaluatorResultType.undefined)
                return;

            if (_type == expressionType.productBased)
            {
                Log.Information("UpdateNotificationExpressionValueCommand");
                var command = new UpdateNotificationExpressionValueCommand()
                {
                    ExpressionValue = bool.Parse(result.Result),
                    BooleanExpressionId = p.Id
                };
                Log.Information(" _mediator.Send(command);");

                await _mediator.Send(command);
            }
        }


        public void DoScheduledWork()
        {
            try
            {
                foreach (var p in GetProductExpressions())
                    DoScheduledWork(p);

                foreach (var i in GetInventoryExpressions())
                    DoScheduledWork(i);

                foreach (var b in GetBooleanExpressions())
                    DoScheduledWork(b);

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);

            }
        }


    }


        public class  EvaluatorResult
        {
                public static EvaluatorResult NewUndefinedResult(
                   
                 ) 
                {
                        var res = new EvaluatorResult();
                        res._result = string.Empty;
                        res._type = EvaluatorResultType.undefined;
                        return res; 
                }

                public static EvaluatorResult NewEvaluatorResult(
            string result
         )
            {
            var res = new EvaluatorResult();
            res._result = result;
            return res;
        }

      
        private string _result = string.Empty;
                private EvaluatorResultType _type = EvaluatorResultType.undefined;

                public EvaluatorResultType Type {
            
                        get
                        {
                            if (Result.ToString().ToUpper().Equals(TRUE) || Result.ToString().ToUpper().Equals("FALSE"))
                                return EvaluatorResultType.boolean;
                   
                            if (decimal.TryParse(Result.ToString(), out _))
                                return EvaluatorResultType.numeric;

                            return EvaluatorResultType.undefined;
                        }
                
                }


        public string Result { get => _result;  set => _result = value; }

        public enum  EvaluatorResultType
                {
                        undefined = 0,
                        numeric = 1,
                        boolean =2, 
                }

        }
    
}
