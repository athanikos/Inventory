using Inventory.Notifications.Contracts;
using Inventory.Notifications.Entities;
using MediatR;
using Serilog;

namespace Inventory.Notifications.Handlers
{
    public class     UpdateNotificationExpressionValue
    : IRequestHandler<UpdateNotificationExpressionValueCommand, NotificationDto>
    {

        private readonly NotifierDbContext _context;

        public UpdateNotificationExpressionValue(NotifierDbContext context) 
        {

            _context = context; 
        }  


       

      async   Task<NotificationDto> IRequestHandler<UpdateNotificationExpressionValueCommand, 
          NotificationDto>.Handle(UpdateNotificationExpressionValueCommand request, CancellationToken cancellationToken)
        {
            Log.Information("UpdateNotificationExpressionValue BooleanExpressionId " + request.BooleanExpressionId);
            Log.Information("UpdateNotificationExpressionValue ExpressionValue " + request.ExpressionValue);


           List<Notification> entities = _context.Notifications.Where(p=>p.BooleanExpressionId == request.BooleanExpressionId).ToList();

            if (entities.Any())
            {
                foreach (var item in entities)
                {
                    item.ExpressionValue = request.ExpressionValue;
                    item.SystemDate = DateTime.Now;
                }
                Log.Information("UpdateNotificationExpressionValue  _context.SaveChangesAsync");
                await _context.SaveChangesAsync();
            }
            //todo: either return nothing or list of dtos
            return new NotificationDto()
            {
                BooleanExpressionId = request.BooleanExpressionId,
                Id = Guid.Empty,
                ExpressionValue = request.ExpressionValue,
                IsActive =false,
                NotifyEveryMinutes = -1,
                NotifyTimes =-1
            } ;
        }
    }
}
