
using MediatR;
using Inventory.Products.Contracts;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Inventory.Expressions;
using Newtonsoft.Json.Linq;
using Inventory.Products.Contracts.Dto;

namespace Expressions
{
    public class Evaluator : IEvaluator
    {
        private const string OpenBracket = "[";
        private const string ClosingBracket = "]";
        private const char Comma = ',';
        private const string OpenParenthesis = "(";
        private const string SUM = "SUM";
        private char[] operators = ['*', '/', '+', '-'];
        private string[] aggregateFunctions = ["SUM", "AVG"];
        private string ALLSpecifier = "[ALL]";

        private string _expression = string.Empty;
        private string _computedExpression = string.Empty;
        private readonly IMediator _mediator;
        private readonly ExpressionsDbContext _context;
        private Guid _inventoryId;


        private List<ProductMetricDto> _products = new List<ProductMetricDto>();
        private List<string?> _productCodes;
        private List<string?> _metricCodes;


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

        public async Task<string> Execute(Guid inventoryId,  string expression)
        {
            _expression = expression;
            _inventoryId = inventoryId;
            GetCodes();
            return await ComputeTokens(ParseTokens());
        }

        private async void GetCodes()
        {
            var response = await _mediator.Send(new CodesQuery());
            if (response == null)
                throw new ArgumentNullException();

            _productCodes = response.ProductCodes;
            _metricCodes = response.MetricCodes;
        }

        private async Task<string> ComputeTokens(List<string> tokens)
        {
            var resultedExpression = string.Empty;
            foreach (var token in tokens)
            {
                if (IsComplexFunction(token))
                    resultedExpression += (await ComputeComplexFunction(token,
                                                 ExtractAggregateFunction(token))).ToString().Trim();
                else if (IsSimpleFunction(token))
                    resultedExpression += (await ComputeSimpleFunction(_inventoryId, token)).ToString().Trim();
                else
                    resultedExpression += token.ToString().Trim();
            }

            return new NCalc.Expression(resultedExpression).Evaluate()
              .ToString();
        }

        private List<string> ParseTokens()
        {
            List<string> resultedList = new List<string>();
            string currentToken = string.Empty;

            for (int i = 0; i < _expression.Length; i++)
            {
                if (operators.Contains(_expression[i]))
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
                    var dto = (await _mediator.
                        Send(new GetProductMetricQuery(_inventoryId, productCode, metricCode
                        , upperboundDate)));

                    if (result != string.Empty)
                        result += "+" + dto.Value;
                    else
                        result += dto.Value;

                    _products.Add(dto);
                }
                else
                    throw new ArgumentException("aggregate function not supported: " + aggregateFunction);

            }
            return result;
        }

        private string  ExtractMetricCode(string token)
        {
            foreach (var item in _metricCodes)
                if (token.Contains(item))
                     return  item;

            throw new ArgumentException(token);
        }

        private List<string> ExtractProductCodes(string token )
        {
            List<string> items = new List<string>();
            if (token.Contains(ALLSpecifier))
                items.Add(ALLSpecifier);
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

            foreach (var item in _productCodes)
                if (token.Contains(item))
                    productCode = item;

            foreach (var item in _metricCodes)
                if (token.Contains(item))
                    metricCode = item;

            return (await _mediator.
                  Send(new GetProductMetricQuery(InventoryId, productCode, metricCode, upperboundDate))).Value.ToString();
        }



        private bool IsSimpleFunction(string token)
        {

            if (IsComplexFunction(token)) return false;

            foreach (var item in _productCodes)
                if (token.Contains(item))
                    return true;

            foreach (var item in _metricCodes)
                if (token.Contains(item))
                    return true;

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool IsComplexFunction(string token)
        {
            foreach (var item in aggregateFunctions)
                if (token.Contains(item))
                    return true;

            if (token.Contains(ALLSpecifier))
                return true;

            var numberOfProducts = 0;
            foreach (var item in _productCodes)
                if (token.Contains(item))
                    numberOfProducts++;
            if (numberOfProducts > 1)
                return true;

            return false;
        }

        public List<Entities.ProductExpression> GetParameters()
        {
            return [.. _context.SingleProductExpressions];
        }


        public void ScedhuleJobs()
        {
            var parameters = GetParameters();

            foreach (var p in parameters)
            {
                RecurringJob.AddOrUpdate(p.Id.ToString(), () => DoScedhuledWork(p), Cron.Minutely);
            }
        }

        public async  void DoScedhuledWork(Entities.ProductExpression p)
        {
            var result =   await  Execute(p.InventoryId, p.Expression);
         
            foreach (var item in _products)
            {
                // inventory metrics 
               

                //var command = new AddProductMetricCommand(p.TargetProductId,
                //   item.MetricId, item.Value, DateTime.Now, item.Currency);
                // 
                // _mediator.Send(command);

                //    Send(new GetProductMetricValueQuery(_inventoryId, item, 
                //      MetricCodeToCompute, upperboundDate))).Value;
            }
        }


        public void DoScedhuledWork()
        {
            foreach (var p in GetParameters())
                DoScedhuledWork(p);
        }

     
    }

}
