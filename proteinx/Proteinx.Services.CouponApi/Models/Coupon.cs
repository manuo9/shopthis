using System.ComponentModel.DataAnnotations;

namespace Proteinx.Services.CouponApi.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }
        [Required]
        public string CouponCode { get; set; }

        public double? DiscountAmount   { get; set; }      

        public int? MinAmount { get; set; }


    }
}
