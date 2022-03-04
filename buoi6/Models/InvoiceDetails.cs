using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buoi6.Models
{
    public class InvoiceDetails
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}
