

namespace Expressions.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using System;
    using Inventory.Expressions;
    using Inventory.Expressions.Dto;
    using Microsoft.AspNetCore.Http;

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
            var entity =
                 new Entities.ProductExpression()
                 {
                     Expression = req.Expression,
                     Id = req.Id,
                     InventoryId = req.InventoryId,
                     RunEveryMinutes = req.RunEveryMinutes,
                     TargetMetricId = req.TargetMetricId,
                     TargetProductId = req.TargetProductId
                 };

            _context.ProductExpressions.Add(entity);
            await _context.SaveChangesAsync();

            return   TypedResults.Ok(new ProductExpressionDto(
                entity.Id,entity.Expression,entity.RunEveryMinutes, entity.TargetProductId,
                entity.TargetMetricId, entity.InventoryId
                ));


        }
    }


    public record AddProductExpressionRequest(Guid Id, 
          string Expression,
          int RunEveryMinutes,
          Guid TargetProductId,
          Guid TargetMetricId,
          Guid InventoryId);







}
