using buoi6.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace buoi6.Data
{
    public class EshopContext: DbContext
    {
        public EshopContext(DbContextOptions<EshopContext> options) : base(options)
        {

        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceDetails> InvoiceDetails { get; set; }
        public DbSet<Cart> Cart { get; set; }
    }
    
}
