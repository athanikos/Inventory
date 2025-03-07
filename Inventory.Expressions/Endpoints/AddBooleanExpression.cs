﻿using FastEndpoints;
using Inventory.Expressions.Dto;
using Inventory.Expressions.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

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
            var entity =
                 new BooleanExpression()
                 {
                     Expression = req.Expression,
                     Id = req.Id,
                     InventoryId = req.InventoryId,
                     RunEveryMinutes = req.RunEveryMinutes,

                 };

            _context.BooleanExpressions.Add(entity);
            await _context.SaveChangesAsync();

            return TypedResults.Ok(new BooleanExpressionDto(
                entity.Id, entity.Expression, entity.RunEveryMinutes, entity.InventoryId
                ));


        }
    }


    public record AddBooleanExpressionRequest(Guid Id,
          string Expression,
          int RunEveryMinutes,
          Guid InventoryId);







}
