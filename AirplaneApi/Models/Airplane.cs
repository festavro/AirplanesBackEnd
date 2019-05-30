using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirplaneApi.Models
{
    public class Airplane
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Model { get; set; }
        public int NumberOfSeats { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
