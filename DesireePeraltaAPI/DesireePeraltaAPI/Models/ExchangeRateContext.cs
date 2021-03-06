using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DesireePeraltaAPI.Models
{
    public class ExchangeRateContext : DbContext
    {
        public ExchangeRateContext(DbContextOptions<ExchangeRateContext> options)
           : base(options)
        {
        }
        public DbSet<ExchangeRate> Currency { get; set; }
    }
}
