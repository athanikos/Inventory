using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Products.Entities
{
    public sealed class
        Metric
    {

        public Guid Id { get; private set; }
            = Guid.NewGuid();

        public string Description { get; private set; }
            = string.Empty;

        public decimal Value { get; private set; }

        public DateTime EffectiveDate { get; private set; }
            = DateTime.MinValue;

        public string Code { get; private set; }
            = string.Empty;


        public ICollection<Product> Products { get; private set; }
            = new List<Product>();

        /// <summary>
        /// System the attribute value came from 
        /// </summary>
        public Guid SourceId { get; set; }
    }
}
