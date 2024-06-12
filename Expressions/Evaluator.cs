using MediatR;
using Inventory.Products.Contracts;
using Microsoft.Identity.Client;

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
        ///      MetricCode(ProductCode,EffectiveDateTime) > 100     
        ///      i.e. Quantity(Ada,Latest) > 100  --> the latest quantity for ada 
        /// </summary>
        public Evaluator(IMediator mediator ,String expression)
        {
            _expression = expression;
            _mediator = mediator;   
        }

        public void Execute()
        {
            GetCodes();
            ComputeTokens(ParseTokens());
           
        }

        public void GetCodes()
        {
             var response =   _mediator.Send(new CodesQuery());
        }

     
        public List<string> ParseTokens()
        {
            return _expression.Split(operators, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
        }

        public void ComputeTokens(List<string> tokens)
        {      
            foreach (var token in tokens)
                if (IsFunction(token))
                {
                   
                }           
        }


        /// <summary>
        ///     i.e. Quantity(Ada,Latest) 
        /// </summary>
        /// <param name="token"></param>
        public void ComputeFunction(string token)
        {
            int firstParenthesis = token.IndexOf(@"(");
            int firstComma = token.IndexOf(@",");
            int productCodeSize = firstComma - firstParenthesis - 1;
            string metricCode = token.Substring(0, firstParenthesis);
            string productCode = token.Substring(firstParenthesis + 1, productCodeSize);
          
            
            var ProductMetricValue = _mediator.Send(new GetProductMetricValueQuery(productCode, metricCode));    


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
