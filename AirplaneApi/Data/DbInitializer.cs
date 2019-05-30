using AirplaneApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirplaneApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AirplaneContext context)
        {
            context.Database.EnsureCreated();
            if (context.Airplanes.Any())
                return;

            var airplanes = new Airplane[]
            {
                new Airplane(){ Code = "A1" , Model = "BOEING 747", NumberOfSeats = 180, CreationDate = DateTime.UtcNow },
                new Airplane(){ Code = "A2" , Model = "AIRBUS A320", NumberOfSeats = 180, CreationDate = DateTime.UtcNow.AddDays(1) },
                new Airplane(){ Code = "A3" , Model = "AIRBUS A320", NumberOfSeats = 180, CreationDate = DateTime.UtcNow.AddDays(2) },
            };

            foreach (var airplane in airplanes)
            {
                context.Airplanes.Add(airplane);
            }

            context.SaveChanges();
        }
    }
}
