using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesireePeraltaAPI.Models
{
    public class ExchangeRate
    {
        public int ID { get; set; }
        public string ISOCode { get; set; }
        public string Description { get; set; }
        public decimal Limit { get; set; }
    }
}
