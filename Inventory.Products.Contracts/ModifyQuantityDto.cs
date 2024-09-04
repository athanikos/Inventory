﻿namespace Inventory.Products.Handlers
{
    public class ModifyQuantityDto
    {
        public Guid ProductId { get; set; }   
        public decimal Diff { get; set; }   
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
    }
}