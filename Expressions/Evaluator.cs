
using MediatR;
using Inventory.Products.Contracts;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Inventory.Expressions;


namespace Expressions
{
    public class Evaluator : IEvaluator
    {
        private const string OpenBracket = "[";
        private const string ClosingBracket = "]";
        private const char Comma = ',';
        private const string OpenParenthesis = "(";
        private const string SUM = "SUM";
        private char[] _operators = ['*', '/', '+', '-'];
        private string[] aggregateFunctions = ["SUM", "AVG"];
        private string ALLSpecifier = "[ALL]";
        private string _expression = string.Empty;
        private readonly IMediator _mediator;
        private readonly ExpressionsDbContext _context;
     
        private List<string> _allProductCodes;
        private List<string> _allMetricCodes;

        private Guid _inventoryId;


        /// <summary>
        ///      todo move to readme    
        ///      MetricCode(ProductCode,EffectiveDateTime) > 100     
        ///      example of expression : Quantity(Ada,Latest) > 100  --> the latest quantity for ada 
        ///      another example : Value(ADA) = PRICE(ADA) * QUANTITY(ADA) 
        ///      this should create a new metric called Value for product ADA 
        ///      effective date should be the date created 
        /// </summary>
        public Evaluator(IMediator mediator, ExpressionsDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<decimal> Execute(Guid inventoryId,  string expression)
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

        private async Task<decimal> ComputeTokens(List<string> tokens)
        {
            var resultedExpression = string.Empty;
            foreach (var token in tokens)
            {
                if (IsOperator(token))   
                    resultedExpression += token.ToString().Trim();
                else if (IsInventoryBasedFormula(token))
                    resultedExpression += (await ComputeComplexFunction(token,
                                                 ExtractAggregateFunction(token))).ToString().Trim();
                else if (IsProductBasedFormula(token))
                    resultedExpression += (await ComputeSimpleFunction(_inventoryId, token)).ToString().Trim();
                
                   
            }

            return decimal.Parse(new NCalc.Expression(resultedExpression).Evaluate()
              .ToString());
        }

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
        private async Task<string> ComputeComplexFunction(string token, string aggregateFunction)
        {
            DateTime upperboundDate = DateTime.MaxValue;
            string metricCode = ExtractMetricCode(token);
            List<string> productCodes = ExtractProductCodes(token);

            string result = string.Empty;
            foreach (var productCode in productCodes)
            {
                if (aggregateFunction == SUM)
                {
                    var dto = (await _mediator.Send(
                               new GetProductMetricQuery(_inventoryId, productCode, metricCode,upperboundDate)));

                      result += result != string.Empty ? "+" : string.Empty + dto.Value;
                }
                else
                    throw new ArgumentException("aggregate function not supported: " + aggregateFunction);

            }
            return result;
        }

        private string  ExtractMetricCode(string token)
        {
            foreach (var item in _allMetricCodes)
                if (token.Contains(item))
                     return  item;

            throw new ArgumentException(token);
        }

        private List<string> ExtractProductCodes(string token )
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
        private async Task<string> ComputeSimpleFunction (Guid InventoryId, string token)
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

            return (await _mediator.
                  Send(new GetProductMetricQuery(InventoryId, productCode, metricCode, upperboundDate))).Value.ToString();
        }

        private formulaType _type = formulaType.undefined;

        public enum formulaType
        {
            undefined = -1, 
            productBased = 0,
            inventoryBased =1
        }

        private bool IsProductBasedFormula(string token)
        {

            if (IsInventoryBasedFormula(token))
                _type  = formulaType.inventoryBased;

            foreach (var item in _allProductCodes)
                if (token.Contains(item))
                    _type = formulaType.productBased;

            foreach (var item in _allMetricCodes)
                if (token.Contains(item))
                    _type = formulaType.productBased;

            return formulaType.productBased == _type;
        }


        private bool IsOperator (string token)
        {
            return token.Length == 1 && _operators.Contains(token[0]);
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
                    _type = formulaType.inventoryBased;
                    return formulaType.inventoryBased == _type;
                }

            if (token.Contains(ALLSpecifier))
            {
                _type = formulaType.inventoryBased;
                return formulaType.inventoryBased == _type;
            }
            var numberOfProducts = 0;
            foreach (var item in _allProductCodes)
                if (token.Contains(item))
                    numberOfProducts++;
            if (numberOfProducts > 1)
                _type = formulaType.inventoryBased;
               
            
            return formulaType.inventoryBased == _type;
        }

        public List<Entities.ProductExpression> GetProductExpressions()
        {
            return [.. _context.ProductExpressions];
        }

        public List<Entities.InventoryExpression> GetInventoryExpressions()
        {
            return [.. _context.InventoryExpressions];
        }


        public void ScheduleJobs(IServiceProvider serviceProvider)
        {
            var productExpressions = GetProductExpressions();
            foreach (var p in productExpressions)
                DoScheduledWork(p);
             

            var inventoryExpressions = GetInventoryExpressions();
            foreach (var i in inventoryExpressions)
                DoScheduledWork(i);
            
        }
        public async void DoScheduledWork(Entities.InventoryExpression p)
        {
            var result = await Execute(p.TargetInventoryId, p.Expression);

            if (_type == formulaType.undefined)
                throw new ArgumentException(_type.ToString());

            if (_type == formulaType.inventoryBased)
            {
                var command = new AddInventoryMetricCommand(p.TargetInventoryId,
                                                    p.TargetMetricId,
                                                    result,
                                                    DateTime.Now, "EUR"); //todo get from prod
                await _mediator.Send(command);
            }
        }

        public async  void DoScheduledWork(Entities.ProductExpression p)
        {
            var result =   await  Execute(p.InventoryId, p.Expression);

            if (_type == formulaType.undefined)
                throw new ArgumentException(_type.ToString());


            if (_type == formulaType.productBased)
            {
                var command = new AddProductMetricCommand(p.TargetProductId,
                                                          p.TargetMetricId,
                                                          result,
                                                          DateTime.Now, "EUR"); //todo get from prod
                await _mediator.Send(command);
            }
        }


        public void DoScheduledWork()
        {
            foreach (var p in GetProductExpressions())
                DoScheduledWork(p);

            foreach (var i in GetInventoryExpressions())
                DoScheduledWork(i);
        }

  
    }

}
