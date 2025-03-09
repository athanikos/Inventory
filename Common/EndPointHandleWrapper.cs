using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using FastEndpoints;
using FluentValidation.Results;

namespace Common
{
    /// <summary>
    /// This class wraps the endpoint handler method and executes it
    /// encloses the method in a try catch block and returns a ProblemDetails object if an exception is thrown  
    /// implementors should use this class to execute the endpoint handler method without worrying about exception handling
    /// </summary>
    /// <typeparam name="Dto"></typeparam>
    /// <typeparam name="Request"></typeparam>
    /// <typeparam name="Ct"></typeparam>
    public class EndPointHandleWrapper<Dto, Request, Ct>
    {
        private readonly Func<Request, Ct, Task<Dto>> _method;
        private readonly Request _req;
        private readonly Ct _ct;

        public EndPointHandleWrapper(Func<Request, Ct, Task<Dto>> method, Request req, Ct ct)
        {
            _method = method;
            _req = req;
            _ct = ct;
        }

        public async Task<Results<Ok<Dto>, NotFound, ProblemDetails>> Execute()
        {
            try
            {
                return TypedResults.Ok(await _method(_req, _ct));
            }
            catch (Exception ex)
            {
                // todo revisit to see if we can use the exception message as the problem details message
                // also log the exception
                return NewProblemDetails(ex, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        private static Results<Ok<Dto>, NotFound, ProblemDetails>
            NewProblemDetails(Exception ex, string message, int StatusCode)
        {
            return new FastEndpoints.ProblemDetails(
            new List<ValidationFailure>()
            {
              new ValidationFailure(message, ex.Message)
            }
           , StatusCode);
        }
    }
}
