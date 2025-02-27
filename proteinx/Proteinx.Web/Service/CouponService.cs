using System.Net.Http;
using System.Net.Http.Headers;
using Proteinx.Web.Models;
using Proteinx.Web.Service.IServices;

namespace Proteinx.Web.Service
{
    public class CouponService : ICouponService             //main business logic
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CouponService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        // recieving token from f
        public void AttachJwtToken(HttpClient _httpClient)
        {

            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Token not found in session.");
            }

        }

        public async Task<List<Coupon>> GetAllCouponsAsync()
        {
            //var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken"); // Get token from session
            //if (!string.IsNullOrEmpty(token))
            //{
            //    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            //}
            AttachJwtToken(_httpClient);
            var response = await _httpClient.GetFromJsonAsync<List<Coupon>>("");
            return response ?? new List<Coupon>();
        }
        //public async Task<List<Coupon>> GetAllCouponsAsync()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<Coupon>>("");
        //}

        public async Task<Coupon> GetCouponByIdAsync(int id)
        {
            AttachJwtToken(_httpClient);
            return await _httpClient.GetFromJsonAsync<Coupon>($"/api/CouponApi/{id}");
          
        }


        public async Task<Coupon> CreateCouponAsync(Coupon coupon)
        {
            AttachJwtToken(_httpClient);
            //serializing to json and sending post req
            var response = await _httpClient.PostAsJsonAsync("", coupon); // in place of "" , we should enter url (API project)

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error creating coupon: {response.StatusCode} - {error}");
            }
            //de-serializing the json (data got from API project which is connected to db)
            var couponR = await response.Content.ReadFromJsonAsync<Coupon>();

            if (couponR == null)
            {
                throw new Exception("Failed to deserialize coupon response.");
            }
            return couponR;
        }

        public async Task<Coupon> UpdateCouponAsync(int id, Coupon coupon)
        {

            AttachJwtToken(_httpClient);

            //System.Diagnostics.Debug.WriteLine($"🛠️ PUT Request URL: api/CouponApi/{id}");
            //System.Diagnostics.Debug.WriteLine($"🛡️ Token: {token}");

            var res = await _httpClient.PutAsJsonAsync($"/api/CouponApi/{id}",coupon);
            
            if (!res.IsSuccessStatusCode)
            {
                var err = res.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error Creating coupon : {res.StatusCode}  - {err}");
            }

             var resRead = await res.Content.ReadFromJsonAsync<Coupon>();

            if (resRead == null)
            {
                throw new Exception("Failed to deserialize coupon response.");
            }
            return resRead;
        }

        public async Task<bool> DeleteCouponAsync(int id)
        {

            AttachJwtToken(_httpClient);
            var res = await _httpClient.DeleteAsync($"/api/CouponApi/{id}");

            if (!res.IsSuccessStatusCode)
            {
                return false;

            }

            return true;


        }

        public async Task<byte[]> ExportCouponsToExcelAsync()
        {
            AttachJwtToken(_httpClient);
            return await _httpClient.GetByteArrayAsync("/api/CouponApi/export");
        }
    }
}
