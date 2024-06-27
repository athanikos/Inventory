using MediatR;

namespace Inventory.Notifications.Contracts
{

    public  class UpdateNotificationExpressionValueCommand
         : IRequest<NotificationDto>
    {
        public Guid BooleanExpressionId { get; set; }

        public bool ExpressionValue { get; set; } 
    }
}
