using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proteinx.Services.CouponApi.Data;
using Proteinx.Services.CouponApi.Models;
using Proteinx.Services.CouponApi.Models.Dtos;

namespace Proteinx.Services.CouponApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CouponApiController : ControllerBase
    {

        private readonly AppDbContext dbContext;
        private readonly IMapper _mapper;

        public CouponApiController(AppDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }
        

        [HttpGet]
        public IActionResult GetAll() {

            var listAll = dbContext.Coupons.ToList();   
            return Ok (listAll);
        }




        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetCouponById(int id)
        {

            var couponById = dbContext.Coupons.Find(id);
            if (couponById == null)
            {
                return NotFound("User id doesn't exists");
            }
            return Ok(couponById);
        }


        [HttpGet("export")]
        public async Task<IActionResult> ExportToExcel()
        {
            var data = dbContext.Coupons.ToListAsync();
            //  Create an Excel workbook
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Coupon"); // Create a worksheet
                int row = 1;
                worksheet.Cell(row, 1).Value = "ID";
                worksheet.Cell(row, 2).Value = "Coupon Code";
                worksheet.Cell(row, 3).Value = "Discount Amount";
                worksheet.Cell(row, 4).Value = "Minimum Amount";


                foreach(var coupon in dbContext.Coupons)
                {
                    row++;
                    worksheet.Cell(row, 1).Value =coupon.CouponId;
                    worksheet.Cell(row, 2).Value = coupon.CouponCode;
                    worksheet.Cell(row, 3).Value = coupon.DiscountAmount;
                    worksheet.Cell(row, 4).Value = coupon.MinAmount;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream); // Save workbook to stream
                    var content = stream.ToArray(); // Convert stream to byte array

                    return File(content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "Coupons.xlsx");
                }

            }



        }


        [HttpPost]
        public IActionResult CreateCoupon(CouponDto couponDto)
        {
            // manually converting dto to entity

            //var coupons = new Coupon()
            //{
            //    CouponCode = couponDto.CouponCode,
            //    DiscountAmount = couponDto.DiscountAmount,
            //    MinAmount = couponDto.MinAmount,

            //};


            // using automapper to convert dto to entity
             var ncoupon= _mapper.Map<Coupon>(couponDto);

            dbContext.Coupons.Add(ncoupon);
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetCouponById), new { id = ncoupon.CouponId }, ncoupon);

        }





        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateCouponById(int id, CouponDto couponDto)
        {

            //  finding coupon by its id 
           var updateCoupon = dbContext.Coupons.Find(id);


            if (updateCoupon == null)
            {
                return NotFound("User id doesn't exist");      
            }

            // manual coded 

            //updateCoupon.CouponCode = couponDto.CouponCode;
            //updateCoupon.DiscountAmount = couponDto.DiscountAmount;
            //updateCoupon.MinAmount = couponDto.MinAmount;


            foreach (var prop in typeof(CouponDto).GetProperties())
            {
                var newValue = prop.GetValue(couponDto);

                // Check if the value is not null (ignore default values)
                if (newValue != null)
                {
                    var entityProp = typeof(Coupon).GetProperty(prop.Name);
                    if (entityProp != null)
                    {
                        entityProp.SetValue(updateCoupon, newValue);
                    }
                }
            }


            // automapper used - not good if you want partial data to be inserted 

            //_mapper.Map(couponDto, updateCoupon, opts =>
            //{
            //    opts.BeforeMap((src, dest) =>
            //    {
            //        foreach (var prop in typeof(CouponDto).GetProperties())
            //        {
            //            var value = prop.GetValue(src);
            //            Console.WriteLine($"Property: {prop.Name}, Value: {value}"); // Debugging log

            //            // Only update properties that are not null
            //            if (value != null)
            //            {
            //                typeof(Coupon).GetProperty(prop.Name)?.SetValue(dest, value);
            //            }
            //        }
            //    });
            //});



            // updateCoupon = _mapper.Map<Coupon>(couponDto);

            dbContext.SaveChanges();
            return Ok(updateCoupon);

        }




        
        //[HttpPatch]
        //[Route("{id:int}")]
        //public IActionResult UpdateCouponByIdPatch(int id, [FromBody] CouponDto couponDto)
        //{
        //    // Find the coupon in the database
        //    var updateCoupon = dbContext.Coupons.Find(id);

        //    if (updateCoupon == null)
        //    {
        //        return NotFound("Coupon ID doesn't exist");
        //    }

        //    // Dynamically update only the provided values
        //    foreach (var prop in typeof(CouponDto).GetProperties())
        //    {
        //        var newValue = prop.GetValue(couponDto);

        //        // Check if the value is not null (ignore default values)
        //        if (newValue != null)
        //        {
        //            var entityProp = typeof(Coupon).GetProperty(prop.Name);
        //            Console.WriteLine($"Property: {prop.Name}, Value: {newValue}"); // Debugging log
        //            if (entityProp != null)
        //            {
        //                entityProp.SetValue(updateCoupon, newValue);
        //            }
        //        }
        //    }

        //    dbContext.SaveChanges();
        //    return Ok(updateCoupon);
        //}


        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteCouponById(int id)
        {

           var toDelete = dbContext.Coupons.Find(id); 
            if (toDelete == null)
            {
                return NotFound("User id doesn't exist");
            }


            dbContext.Coupons.Remove(toDelete);
            dbContext.SaveChanges();

            return NoContent();
        }                                                                                                                                                                   



            
                        



    }
}
