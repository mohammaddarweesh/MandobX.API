using AutoMapper;
using MandobX.API.Models;
using MandobX.API.ViewModels;

namespace MandobX.Helpers
{
    public class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<ShipmentOperation, ShipmentOperationViewModel>()
                .ForMember(dest=>dest.Driver, opt=>opt.MapFrom(src=>src.Driver.User.UserName))
                .ForMember(dest=>dest.CreationDate, opt=>opt.MapFrom(src=>src.CreationDate))
                .ForMember(dest=>dest.FromRegion, opt=>opt.MapFrom(src=>src.FromRegion.Name))
                .ForMember(dest=>dest.ToRegion, opt=>opt.MapFrom(src=>src.ToRegion.Name))
                .ForMember(dest=>dest.PackageType, opt=>opt.MapFrom(src=>src.PackageType.Name))
                .ForMember(dest=>dest.ShipmentDate, opt=>opt.MapFrom(src => src.ShipmentDate))
                .ForMember(dest=>dest.Trader, opt=>opt.MapFrom(src=>src.Trader.User.UserName));
        }
    }
}
