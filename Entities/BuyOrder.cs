using System.ComponentModel.DataAnnotations;
using UnitTesting_UDEMY.Validators;
namespace Entities.Models
{
    public class BuyOrder
    {
        public Guid BuyOrderID { get; set; }

        [Required]
        public required string StockSymbol { get; set; }

        [Required]
        public required string StockName { get; set; }

        [DateValidator]
        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "{0} should be between 1 and 100000.")]
        public int Quantity { get; set; }

        [Range(1, 100000, ErrorMessage = "{0} should be between 1 and 100000.")]
        public double Price { get; set; }
    }
}
