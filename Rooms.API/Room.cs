using Microsoft.AspNetCore.Authentication;

namespace Rooms.API
{
    public class Room
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public Guid InventoryId { get; set; }
    }

    public class Inventory
    {
        public Guid Id { get; set; }

        public required string Description { get; set; }

        public required string Code { get; set; }


    }
}
