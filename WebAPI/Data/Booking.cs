using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Data
{
    public class Booking
    {
        public int Id { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string TypeOfGood { get; set; }
        public DateTime BookingDateTime { get; set; }
        public string Weight { get; set; }
        public string PricingType { get; set; }
        
        public bool IsActive { get; set; }

        public Driver Driver { get; set; }
        public int? DriverId { get; set; }

    }
}
