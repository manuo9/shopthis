using AutoMapper;
using Proteinx.Services.CouponApi.Models;
using Proteinx.Services.CouponApi.Models.Dtos;

namespace Proteinx.Services.CouponApi.Profiles
{
    public class MappingProfile : Profile
    {

        public MappingProfile() {
            // bi directional convertion using reversemap()
            CreateMap<CouponDto,Coupon>().ReverseMap();
        }

    }
}
