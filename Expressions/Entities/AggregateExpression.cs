using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Expressions.Entities
{


    // <summary>
    // TOTAL VALUE = SUM( VALUE (ADA,UpperBoundDate) )
    //  or
    //  VALUE([ADA, XRP] , UpperBoundDate) )
    //  or
    //  VALUE(ALL , UpperBound)
    // </summary>
    public  class AggregateExpression
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public Guid MetricId { get; set; }

        public Guid ProductId { get; set; }

        public int RunEveryMinutes { get; set; }
    }
}
