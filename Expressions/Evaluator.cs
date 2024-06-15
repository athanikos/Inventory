using MediatR;
using Inventory.Products.Contracts;

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
            
            int nextToendOfProductCodeIndex = token.IndexOf(@",");
       
            if (nextToendOfProductCodeIndex== -1)
            {
                nextToendOfProductCodeIndex = token.IndexOf(@")");
            }

            int productCodeSize = nextToendOfProductCodeIndex - firstParenthesis - 1;
            string metricCode = token.Substring(0, firstParenthesis);
            string productCode = token.Substring(firstParenthesis + 1, productCodeSize);
                   
            return (await _mediator.
                   Send(new GetProductMetricValueQuery(productCode, metricCode, DateTime.Now))).Value;

        }

     

        public bool IsFunction(string token ) {
            foreach (var item in _productCodes)
                if (token.Contains(item))
                    return true;

            foreach (var item in _metricCodes)
                if (token.Contains(item))
                    return true;

            return false;   
        }



    }
}
