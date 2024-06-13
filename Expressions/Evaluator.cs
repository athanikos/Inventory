using MediatR;
using Inventory.Products.Contracts;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Expressions
{
    public class Evaluator
    {

        private string _expression = string.Empty;
        private string _computedExpression = string.Empty;
        private readonly IMediator _mediator; 
        private char[] operators = new char[] { '*', '/', '+', '-' };
        private List<string> _productCodes = new List<string>();    
        private List<string> _metricCodes = new List<string>();


        /// <summary>
        ///      todo move top readme    
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

        public void GetCodes()
        {
             var response =   _mediator.Send(new CodesQuery());
        }

     
        public List<string> ParseTokens()
        {
            return _expression.Split(operators, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
        }

        public async Task<string> ComputeTokens(List<string> tokens)
        {      
            var resultedExpression = string.Empty;
            foreach (var token in tokens)
                if (IsFunction(token))
                    resultedExpression += (await  ComputeFunction(token)).ToString();
                else
                   resultedExpression += token.ToString();   
            
            return resultedExpression;  
        }


        /// <summary>
        ///     Parses a formual of the following   Quantity(Ada,Latest)
        ///     and gets the value in product metric table 
        ///     and returns that value 
        /// </summary>
        /// <param name="token"></param>
        public async Task<decimal> ComputeFunction(string token)
        {
            int firstParenthesis = token.IndexOf(@"(");
            int firstComma = token.IndexOf(@",");
            int productCodeSize = firstComma - firstParenthesis - 1;
            string metricCode = token.Substring(0, firstParenthesis);
            string productCode = token.Substring(firstParenthesis + 1, productCodeSize);
                   
            return (await _mediator.
                   Send(new GetProductMetricValueQuery(productCode, metricCode, DateTime.Now))).Value;

        }

     

        public bool IsFunction(string token ) {
            foreach (var item in _productCodes)
                if (item.Contains(token))
                    return true;

            foreach (var item in _metricCodes)
                if (item.Contains(token))
                    return true;

            return false;   
        }



    }
}
