using AutoMapper;
using MandobX.API.Authentication;
using MandobX.API.Models;
using MandobX.API.ViewModels;

namespace MandobX.Helpers
{
    /// <summary>
    /// auto mapper helper
    /// </summary>
    public class AutoMapper:Profile
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AutoMapper()
        {
            CreateMap<CreateShipmentViewModel, GoogleMap>();
            CreateMap<CreateShipmentViewModel, ShipmentOperation>();
            CreateMap<ShipmentOperation, ShipmentListViewModel>()
                .ForMember(dest => dest.Driver, opt => opt.MapFrom(src => src.Driver.User.UserName))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.FromRegion, opt => opt.MapFrom(src => src.FromRegion.Name))
                .ForMember(dest => dest.ToRegion, opt => opt.MapFrom(src => src.ToRegion.Name))
                .ForMember(dest => dest.PackageType, opt => opt.MapFrom(src => src.PackageType.Name))
                .ForMember(dest => dest.ShipmentDate, opt => opt.MapFrom(src => src.ShipmentDate))
                .ForMember(dest => dest.Trader, opt => opt.MapFrom(src => src.Trader.User.UserName));
            CreateMap<CreateVehicleViewModel, Vehicle>();
            CreateMap<EditShipmentViewModel, ShipmentOperation>();
            CreateMap<ShipmentOperation, EditShipmentViewModel>();
            CreateMap<Driver, EditDriverProfileViewModel>()
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
            CreateMap<Trader, EditTraderProfileViewModel>()
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.TypeOftradingId, opt => opt.MapFrom(src => src.TypeOftradingId));
            CreateMap<Driver, DriverCreateShipmentViewModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Points))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.VehicleBrand, opt => opt.MapFrom(src => src.Vehicle.CarBrand.Name))
                .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.Vehicle.Id))
                .ForMember(dest => dest.VehicleNumber, opt => opt.MapFrom(src => src.Vehicle.Number))
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.Vehicle.CarType.Name))
                .ForMember(dest => dest.VehicleVolume, opt => opt.MapFrom(src => src.Vehicle.Volume));
        }
    }
}
