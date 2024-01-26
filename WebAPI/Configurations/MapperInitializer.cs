using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Configurations
{
    public class MapperInitializer : Profile
    {
        /// <summary>
        /// Bridge between data classes and DTOs
        /// </summary>
        public MapperInitializer()
        {
            CreateMap<APIUser, UserDTO>().ReverseMap();
            CreateMap<APIUser, LoginUserDTO>().ReverseMap();

            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<Booking, CreateBookingDTO>().ReverseMap();

            CreateMap<Driver, DriverDTO>().ReverseMap();
           
        }
    }
}
