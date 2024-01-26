using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    //public class BookingDTO
    //{
    //    public int Id { get; set; }
    //    public string FromAddress { get; set; }
    //    public string ToAddress { get; set; }
    //    public string TypeOfGood { get; set; }
    //    public DateTime BookingDateTime { get; set; }
    //    public string Weight { get; set; }
    //    public string PricingType { get; set; }
    //    public int DriverId { get; set; }
    //    public bool IsActive { get; set; }
    //}

    public class CreateBookingDTO
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string TypeOfGood { get; set; }
        public DateTime BookingDateTime { get; set; }
        public string Weight { get; set; }
        public string PricingType { get; set; }
        public bool IsActive { get; set; }
        public int? DriverId { get; set; }
    }

    public class BookingDTO : CreateBookingDTO
    {
        public int Id { get; set; }

        public DriverDTO Driver { get; set; }

    }
}
