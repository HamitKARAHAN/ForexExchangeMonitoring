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
        [Column("currency_Id")]
        public int CurrencyModelId { get; set; }

        [Column("currency_name")]
        public string CurrencyName{ get; set; }

        public List<ForexCurrencyModel> FromCurrencyIds{ get; set; }
        public List<ForexCurrencyModel> ToCurrencyIds { get; set; }
    }
}
