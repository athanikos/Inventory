using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Inventory.Notifications.Contracts;
using Microsoft.EntityFrameworkCore;
using Inventory.Expressions.Contracts;
using Microsoft.AspNetCore.Http;

namespace Inventory.Notifications.Endpoints
{
    public class GetNotifications :
        Endpoint<GetNotificationsRequest>
    {
        private readonly IMediator _mediator;
        private readonly NotifierDbContext _context;

        public GetNotifications(IMediator mediator, NotifierDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public override void Configure()
        {
            Get("/Notification"); 

            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task
            HandleAsync(GetNotificationsRequest req,
                        CancellationToken ct)
        {

       
           var booleanExpressions =    await           
                _mediator.Send(new GetBooleanExpressionsQuery());

            var Notifications = await _context.Notifications.
                                      ToListAsync();

            await SendAsync(Notifications.Join(
                booleanExpressions,
                n => n.BooleanExpressionId, be => be.Id,
                (n, be) =>
                new NotificationDto()
                {
                    BooleanExpressionId = n.BooleanExpressionId,
                    ExpressionValue = n.ExpressionValue,
                    Id = n.Id,
                    IsActive = n.IsActive,
                    NotifyEveryMinutes = n.NotifyEveryMinutes,
                    NotifyTimes = n.NotifyTimes,
                    Expression = be.Expression
                }
                ).ToList()
                );


          
        }
    }



    public record GetNotificationsRequest(bool isActive);
}
