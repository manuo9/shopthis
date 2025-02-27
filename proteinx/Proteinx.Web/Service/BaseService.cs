using System.Net.Http.Headers;

namespace Proteinx.Web.Service
{
    public abstract class BaseService
    {
        private IHttpContextAccessor _httpContextAccessor;

        protected BaseService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        protected void AttachJwtToken(HttpClient httpClient)
        {


            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Token not found in session.");
            }


        }



    }


}
