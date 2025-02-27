using Microsoft.EntityFrameworkCore;
using Proteinx.Services.CouponApi.Models;

namespace Proteinx.Services.CouponApi.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }
       

    }
}
