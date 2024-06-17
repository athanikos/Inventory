
using MediatR;
using Inventory.Products.Contracts;
using System.Collections.Generic;
using NCalc;

namespace Expressions
{
    public class Evaluator
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

        private List<string> _productCodes = new List<string>();    
        private List<string> _metricCodes = new List<string>();
        /// <summary>
        ///      todo move to readme    
        ///      MetricCode(ProductCode,EffectiveDateTime) > 100     
        ///      example of expression : Quantity(Ada,Latest) > 100  --> the latest quantity for ada 
        ///      another example : Value(ADA) = PRICE(ADA) * QUANTITY(ADA) 
        ///      this should create a new metric called Value for product ADA 
        ///      effective date should be the date created 
        /// </summary>
        public Evaluator(IMediator mediator ,String expression)
        {
            _expression = expression;
            _mediator = mediator;   
        }

        public async Task<string> Execute()
        {
            GetCodes();
            return  await  ComputeTokens(ParseTokens());
        }

        public async void GetCodes()
        {
             var response =  await  _mediator.Send(new CodesQuery());
             if (response == null)
                throw new ArgumentNullException();

            _productCodes = response.ProductCodes;
            _metricCodes = response.MetricCodes;    

        }

     
        public List<string> ParseTokens()
        {
            List<string> resultedList = new List<string>();
            string currentToken = string.Empty;    
            
            for (int i = 0; i < _expression.Length; i++)
            {
                if (operators.Contains(_expression[i]))
                {
                    if(currentToken!=string.Empty)
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

        public async Task<string> ComputeTokens(List<string> tokens)
        {      
            var resultedExpression = string.Empty;
            foreach (var token in tokens)
            {
                 if (IsComplexFunction(token))
                    resultedExpression += (await ComputeComplexFunction(token,
                                                 ExtractAggregateFunction(token))).ToString().Trim();
                else if (IsSimpleFunction(token))
                    resultedExpression += (await ComputeSimpleFunction(token)).ToString().Trim();
                else
                    resultedExpression += token.ToString().Trim();
            }

            var expression = new Expression(resultedExpression);
            if (expression == null)
                return string.Empty;
            return expression.Evaluate().ToString();  
        }

        public string ExtractAggregateFunction(string token)
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
        public async Task<string> ComputeComplexFunction(string token, string aggregateFunction)
        {
            DateTime upperboundDate = DateTime.MaxValue;    
            List<string> productCodesToCompute = new List<string>();
            string metricCode = string.Empty;

            foreach (var item in _metricCodes)
                if (token.Contains(item))
                    metricCode = item;
            
            if (token.Contains(ALLSpecifier))
                productCodesToCompute.AddRange(_productCodes);
            else
                productCodesToCompute.AddRange(ExtractProducts(token).Split(Comma));
      
            string result = string.Empty;
            foreach (var productCode in productCodesToCompute)
            {
                if (aggregateFunction == SUM)
                {
                    if (result!=string.Empty)
                         result += "+" + (await _mediator.
                                   Send(new GetProductMetricValueQuery(productCode, metricCode, upperboundDate))).Value;

                    else 
                        result += (await _mediator.
                        Send(new GetProductMetricValueQuery(productCode, metricCode, upperboundDate))).Value;

                }
                else
                {
                    throw new ArgumentException(); 
                }
            }
            return result;
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
        public async Task<string> ComputeSimpleFunction(string token)
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
                   Send(new GetProductMetricValueQuery(productCode, metricCode, upperboundDate))).Value.ToString();
        }


       
        public bool IsSimpleFunction(string token ) {
        
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
        public bool IsComplexFunction(string token)
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
            if (numberOfProducts>1)
                return true;
                        
            return false;
        }
    }

}
