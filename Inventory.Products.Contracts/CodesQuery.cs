﻿using MediatR;

namespace Inventory.Products.Contracts
{
    public  record CodesQuery(Guid InventoryId) :
        IRequest<CodesResponse>;
    public class CodesResponse
    {
        public List<string> ProductCodes { get; set; } = new();
        public List<string> MetricCodes { get; set; } = new();
    }


}
