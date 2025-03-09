using FastEndpoints;
using FluentValidation.Results;
using Inventory.Expressions.Dto;
using Inventory.Expressions.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Expressions.Endpoints
{
    public class AddProductExpression :
        Endpoint<AddProductExpressionRequest>
    {
        private readonly IMediator _mediator;
        private readonly ExpressionsDbContext _context;

        public AddProductExpression(IMediator mediator, ExpressionsDbContext context)
        {
            _mediator = mediator;
            _context = context;    
        }

        public override void Configure()
        {
            Post("/ProductExpression");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok<ProductExpressionDto>, NotFound, ProblemDetails>>
           HandleAsync(AddProductExpressionRequest req,
                       CancellationToken ct)
        {
            try
            {
                var entity = AddProductExpressionRequestExtensions.ToEntity(req);
                _context.ProductExpressions.Add(entity);
                await _context.SaveChangesAsync(ct);
                var dto = ProductExpressionDtoExpressions.ToDto(entity);
                return TypedResults.Ok(dto);
            }
            catch (DbUpdateException ex)
            {
               return NewProblemDetails(ex, ex.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return NewProblemDetails(ex, ex.Message, StatusCodes.Status500InternalServerError);

            }
        }


        private static Results<Ok<ProductExpressionDto>, NotFound, ProblemDetails>
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

    public static class ProductExpressionDtoExpressions
    {
   
        public static ProductExpressionDto ToDto(this ProductExpression entity)
        {
            return new ProductExpressionDto(
                entity.Id, entity.Expression, entity.RunEveryMinutes, entity.TargetProductId,
                entity.TargetMetricId, entity.InventoryId);
        }

    }

    public static class AddProductExpressionRequestExtensions
    {
        public static ProductExpression ToEntity(this AddProductExpressionRequest req)
        {
            return new ProductExpression()
            {
                Expression = req.Expression,
                Id = req.Id,
                InventoryId = req.InventoryId,
                RunEveryMinutes = req.RunEveryMinutes,
                TargetMetricId = req.TargetMetricId,
                TargetProductId = req.TargetProductId
            };
        }   
    }

    public record AddProductExpressionRequest(Guid Id, 
          string Expression,
          int RunEveryMinutes,
          Guid TargetProductId,
          Guid TargetMetricId,
          Guid InventoryId);







}
