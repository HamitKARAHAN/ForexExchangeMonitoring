using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForexExchange.Models
{
    public class RealTimeCurrencyExchangeRate
    {
        [Key]
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public long Rate { get; set; }
        public DateTime LastModifiedDate { get; set; }

    }
}
