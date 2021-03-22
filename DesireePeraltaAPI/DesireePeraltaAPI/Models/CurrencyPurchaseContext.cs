using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DesireePeraltaAPI.Models
{
    public class CurrencyPurchaseContext : DbContext
    {
        public CurrencyPurchaseContext(DbContextOptions<CurrencyPurchaseContext> options)
            : base(options)
        {
        }
        public DbSet<CurrencyPurchase> CurrencyPurshaseItems { get; set; }
    }
}
