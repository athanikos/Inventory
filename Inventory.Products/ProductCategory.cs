using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Products
{

    /// <summary>
    /// tree like representation of product categories 
    /// root nodes have FatherId empty 
    /// </summary>
    public  class ProductCategory
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name { get; private set; } = string.Empty;

        public Guid FatherId { get; private set; } = Guid.Empty;

        public ICollection<Product> Products { get; private set; } = new List<Product>();

    }
}
