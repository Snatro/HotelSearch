using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearch.Domain
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public decimal Price { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
