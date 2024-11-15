using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class StockTrade
    {
        [Required]
        public string StockSymbol { get; set; }

        [Required]
        public string StockName { get; set; }

        [Range(1, 100000, ErrorMessage = "{0} should be between 1 and 100000.")]
        public int Quantity { get; set; }

        [Range(1, 100000, ErrorMessage = "{0} should be between 1 and 100000.")]
        public double Price { get; set; }
    }
}
