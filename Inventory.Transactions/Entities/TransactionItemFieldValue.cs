using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Transactions.Entities
{

    /// <summary>
    /// represents the actual computed value for a transaction field 
    /// </summary>
    public  class TransactionItemFieldValue
    {
        public Guid  Id { get; set; }

        public decimal Value { get; set; }


        // todo do i need this?
        public required TransactionItemTemplateField Field { get; set; } 

        public Guid FieldId { get; set; }
    }
}
