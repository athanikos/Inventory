namespace Inventory.Expressions
{
    public class  EvaluatorResult
    {
        private string _result = string.Empty;
        private readonly EvaluatorResultType _type = EvaluatorResultType.undefined;
        private const string TRUE = "TRUE";
        private const string FALSE = "FALSE";

        public static EvaluatorResult NewUndefinedResult() 
        {
            var res = new EvaluatorResult
            {
                _result = string.Empty
            };
            return res; 
        }

        public static EvaluatorResult NewEvaluatorResult(string result)
        {
            var res = new EvaluatorResult
            {
                _result = result
            };
            return res;
        }
     
        public EvaluatorResultType Type 
        { 
            get
            {
                if (Result.ToString().ToUpper().Equals(TRUE) || Result.ToString().ToUpper().Equals("FALSE"))
                    return EvaluatorResultType.boolean;
                   
                if (decimal.TryParse(Result.ToString(), out _))
                   return EvaluatorResultType.numeric;

                return EvaluatorResultType.undefined;
            }
        }

        public string Result 
        {
                               get => _result; 
                               set => _result = value;
        }

        public enum  EvaluatorResultType
        {
                        undefined = 0,
                        numeric = 1,
                        boolean =2, 
        }

    }
    
}
