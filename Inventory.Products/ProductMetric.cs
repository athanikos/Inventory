using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Products
{

    /// <summary>
    ///  this can be any attribute required 
    /// </summary>
    public sealed class ProductMetric
    {

        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Description { get; private set; } = string.Empty;

        public long ProductId { get; private set; }

        public decimal Value { get; private set; }

        public DateTime EffectiveDate { get; private set; } = DateTime.MinValue; 

        public string Code { get; private set; } = string.Empty;

        /// <summary>
        /// System the attribute value came from 
        /// </summary>
        public Guid SourceId { get; set; }
    }
}
