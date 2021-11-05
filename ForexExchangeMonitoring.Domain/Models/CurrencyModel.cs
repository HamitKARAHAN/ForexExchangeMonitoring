using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexExchangeMonitoring.Domain.Models
{
    public class CurrencyModel
    {
        [Key]
        public int CurrencyModelId { get; set; }
        public string CurrencyName{ get; set; }
    }
}
