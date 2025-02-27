using System.ComponentModel.DataAnnotations;

namespace Proteinx.Services.CouponApi.Models.Dtos
{
    public class CouponDto
    {
    
        [Required]
        public string CouponCode { get; set; }

       
        public double? DiscountAmount { get; set; }

        public int? MinAmount { get; set; }

    }
}
