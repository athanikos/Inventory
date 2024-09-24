using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Transactions.Entities
{
    /// <summary>
    /// represents a line in a section where all fields are grouped together 
    /// entity based section will have one group most of the time 
    /// product based sections will mostly have multiple lines of products 
    /// </summary>
    public  class TransactionSectionGroup
    {
        public Guid Id { get; set; }
        public Guid TransactionSectionId { get; set; }
        public int GroupValue { get; set; }
        public ICollection<Value> Values { get; set; } = [];
        public required TransactionSection TransactionSection { get; set; }


    }
}
