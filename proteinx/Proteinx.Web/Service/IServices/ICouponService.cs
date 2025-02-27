using System.Net.Http;
using Proteinx.Web.Models;

namespace Proteinx.Web.Service.IServices
{
    public interface ICouponService
    {

        Task<List<Coupon>> GetAllCouponsAsync();
        Task<Coupon> GetCouponByIdAsync(int id);
        Task<Coupon> CreateCouponAsync(Coupon coupon);
        Task<Coupon> UpdateCouponAsync(int id,Coupon coupon);
        Task<bool> DeleteCouponAsync(int id);

        Task<byte[]> ExportCouponsToExcelAsync();
        
           
        


    }
}
