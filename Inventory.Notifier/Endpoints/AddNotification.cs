

namespace Inventory.Notifications.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using System;
    using Inventory.Notifications.Contracts;
    using Inventory.Prices;
    using Microsoft.AspNetCore.Http;

    public class AddNotification :
        Endpoint<AddNotificationRequest>
    {
        private readonly IMediator _mediator;
        private readonly NotifierDbContext _context;

        public AddNotification(IMediator mediator, NotifierDbContext context )
        {
            _mediator = mediator;
            _context = context;
        }

        public override void Configure()
        {
            Post("/Notification");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok<NotificationDto>, NotFound, ProblemDetails>>
            HandleAsync(AddNotificationRequest req,
                        CancellationToken ct)
        {

            var entity =
            new Entities.Notification()
            {
              BooleanExpressionId = req.BooleanExpressionId,
              ExpressionValue = req.ExpressionValue,    
              Id = req.Id,
              IsActive = req.IsActive,
              NotifyEveryMinutes = req.NotifyEveryMinutes,
              NotifyTimes = req.NotifyTimes 
            };

            _context.Notifications.Add(entity);
            await _context.SaveChangesAsync();

            return TypedResults.Ok(new NotificationDto( )
            {
                BooleanExpressionId    = req.BooleanExpressionId,  
                NotifyTimes = req.NotifyTimes,
                NotifyEveryMinutes = req.NotifyEveryMinutes,    
                IsActive=req.IsActive,
                Id = req.Id,
                ExpressionValue = req.ExpressionValue   
            });
              

        }
    }



    public record AddNotificationRequest(Guid Id, Guid BooleanExpressionId,
                                          bool ExpressionValue, int NotifyEveryMinutes,
                                          int NotifyTimes, bool IsActive );

    

}
