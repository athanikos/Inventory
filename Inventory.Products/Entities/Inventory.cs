﻿
namespace Inventory.Products.Entities
{

    public sealed class Inventory
    {
        public Guid Id { get; set; }
         = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public ICollection<Product> Products
        { get; set; } = new List<Product>();


        public ICollection<Metric> Metrics { get; set; } = new List<Metric>();

    }

}