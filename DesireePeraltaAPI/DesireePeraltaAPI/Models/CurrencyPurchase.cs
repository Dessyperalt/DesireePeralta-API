using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesireePeraltaAPI.Models
{
    public class CurrencyPurchase
    {
        public int ID { get; set; }
        public int userID { get; set; }
        public decimal amount { get; set; }
        public string currencyCode { get; set; }
        public decimal exchangeRate { get; set; }
        public decimal total { get; set; }
        public DateTime date { get; set; }
    }
}
