using Common;
using FastEndpoints;
using Inventory.Expressions.Dto;
using Inventory.Expressions.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Expressions.Endpoints
{
    public class AddBooleanExpression :
        Endpoint<AddBooleanExpressionRequest>
    {
        private readonly IMediator _mediator;
        private readonly ExpressionsDbContext _context;

        public AddBooleanExpression(IMediator mediator,
            ExpressionsDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public override void Configure()
        {
            Post("/BooleanExpression");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok<BooleanExpressionDto>, NotFound, ProblemDetails>>
            HandleAsync(AddBooleanExpressionRequest req,
                        CancellationToken ct)
        {
            return await new EndPointHandleWrapper<BooleanExpressionDto,
                            AddBooleanExpressionRequest,
                            CancellationToken>(Handle, req, ct).
                            Execute();
        }

        private async Task<BooleanExpressionDto>
            Handle(AddBooleanExpressionRequest req, CancellationToken ct)
        {
            var entity = AddBooleanExpressionRequestExtensions.ToEntity(req);
            _context.BooleanExpressions.Add(entity);
            await _context.SaveChangesAsync();
            return BoooleanExpressionExtensions.ToDto(entity);
        }
    }

    public record AddBooleanExpressionRequest(Guid Id,
          string Expression,
          int RunEveryMinutes,
          Guid InventoryId);

    public static class BoooleanExpressionExtensions
    {
        public static BooleanExpressionDto ToDto(this BooleanExpression entity)
        {
            return new BooleanExpressionDto(
                            entity.Id, entity.Expression, entity.RunEveryMinutes, entity.InventoryId
                            );
        }
    }

    public static class AddBooleanExpressionRequestExtensions
    {
        public static BooleanExpression ToEntity(this AddBooleanExpressionRequest req)
        {
            return new BooleanExpression()
            {
                Expression = req.Expression,
                Id = req.Id,
                InventoryId = req.InventoryId,
                RunEveryMinutes = req.RunEveryMinutes,
            };
        }
    }
}
