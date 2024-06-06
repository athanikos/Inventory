﻿namespace Inventory.Products.Entities
{
    public sealed class
        Metric
    {

        public Guid Id { get; internal set; }
            = Guid.NewGuid();

        public string Description { get; internal set; }
            = string.Empty;



        public string Code { get; internal set; }
            = string.Empty;


        public ICollection<Product> Products { get; internal set; }
            = new List<Product>();

        /// <summary>
        /// System the attribute value came from 
        /// </summary>
        public Guid SourceId { get; set; }
    }
}
