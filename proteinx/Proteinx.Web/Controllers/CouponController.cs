using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proteinx.Web.Models;
using Proteinx.Web.Service.IServices;

namespace Proteinx.Web.Controllers
{

    public class CouponController : Controller               // Calling methods & returning views()
    {

        private readonly ICouponService _couponService;


        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> Index()
        {
            var coupons = await _couponService.GetAllCouponsAsync();
            return View(coupons);
        }


        public async Task<IActionResult> DetailsByIndex(int id)
        {
            var coupon = await _couponService.GetCouponByIdAsync(id);
            return View(coupon);
        }



        // Create Coupon (GET Form)
        public IActionResult Create()
        {
            return View();
        }

        // Create Coupon (POST Request)
        [HttpPost]
        public async Task<IActionResult> Create(Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                return View(coupon);
            }

            await _couponService.CreateCouponAsync(coupon);
            return RedirectToAction(nameof(Index)); // redirecting to index.cshtml page
        }


        // Update Coupon (GET Form)
        public async Task<IActionResult> Edit(int id)
        {
            var coupons = await _couponService.GetAllCouponsAsync();
            var coupon = coupons.FirstOrDefault(c => c.CouponId == id);  //getting specific from list (all coupons) through id

            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        // exporting data in .xlsx
        public async Task<IActionResult> DownloadExcel()
        {
            var fileBytes = await _couponService.ExportCouponsToExcelAsync();
            return File(fileBytes,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Coupons.xlsx");
        }



        // Update Coupon (POST Request)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Coupon coupon)
        {
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Edit POST called with ID: {id}"); // Debug log
            if (!ModelState.IsValid)
            {
                return View(coupon);
            }
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Calling UpdateCouponAsync with ID: {id}"); // Debug log

            await _couponService.UpdateCouponAsync(id, coupon);

            System.Diagnostics.Debug.WriteLine($"[DEBUG] Update completed, redirecting to Index"); // Debug log

            return RedirectToAction(nameof(Index));
        }
    

    [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
           await _couponService.DeleteCouponAsync(id);

            return RedirectToAction(nameof(Index));

        }












    } 


}

