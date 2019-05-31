using AirplaneApi.Controllers;
using AirplaneApi.Data;
using AirplaneApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Tests
{
    public class AirplaneControllerTests
    {
        private AirplanesController controller;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AirplaneContext>()
                .UseInMemoryDatabase(databaseName: "Gol")
                .Options;
            var context = new AirplaneContext(options);
            context.Airplanes.Add(new Airplane() { Id = 1, Code = "A1", Model = "M1", NumberOfSeats = 180, CreationDate = DateTime.UtcNow });
            context.Airplanes.Add(new Airplane() { Id = 2, Code = "A2", Model = "M2", NumberOfSeats = 100, CreationDate = DateTime.UtcNow });
            context.Airplanes.Add(new Airplane() { Id = 3, Code = "A3", Model = "M3", NumberOfSeats = 200, CreationDate = DateTime.UtcNow });
            context.SaveChanges();
            this.controller = new AirplanesController(context);
        }

        [Category("GetAirplanes")]
        [Test]
        public void ShouldReturnAllAirplanes()
        {
            var airplanes = controller.GetAirplanes();
            Assert.AreEqual(3, airplanes.Count());
        }

        [Category("GetAirplane")]
        [Test]
        public void ShouldReturnAirplane()
        {
            var airplaneId = 1;
            var airplane = controller.GetAirplane(airplaneId);
            AssertAirplane1(airplane.Result.Value);
        }

        [Category("GetAirplane")]
        [Test]
        public async Task ShouldReturnNotFoundWhenAirplaneDoesNotExistAsync()
        {
            var airplaneId = 10;
            var airplane = await controller.GetAirplane(airplaneId);
            var objectResponse = airplane.Result as StatusCodeResult;
            Assert.AreEqual(objectResponse.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [Category("GetAirplane")]
        [Test]
        public async Task ShouldReturnBadRequestWhenIdIsNegative()
        {
            var airplaneId = -10;
            var airplane = await controller.GetAirplane(airplaneId);
            var objectResponse = airplane.Result as ObjectResult;
            Assert.AreEqual(objectResponse.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Category("PostAirplane")]
        [Test]
        public async Task ShouldSaveNewAirplane()
        {
            var airplane = new Airplane() { Id = 4, Code = "New1", Model = "New1", CreationDate = DateTime.UtcNow, NumberOfSeats = 30 };
            var response = await controller.PostAirplane(airplane);
            var newAirplane = controller.GetAirplane(airplane.Id);
            Assert.NotNull(newAirplane.Result.Value);
        }

        [Category("PutAirplane")]
        [Test]
        public async Task ShouldUpdateAirplane()
        {
            var airplaneId = 4;
            var airplane = new Airplane() { Id= airplaneId, Code = "Updated1", Model = "Updated1", NumberOfSeats = 40 };
            var response = await controller.PutAirplane(airplaneId , airplane);
            var newAirplane = controller.GetAirplane(airplane.Id).Result.Value;
            Assert.AreEqual(newAirplane.Code, "Updated1");
            Assert.AreEqual(newAirplane.Model, "Updated1");
            Assert.AreEqual(newAirplane.NumberOfSeats, 40);
        }

        [Category("PutAirplane")]
        [Test]
        public async Task ShouldReturnBadRequestWhenIdIsNegativeForUpdate()
        {
            var airplaneId = -10;
            var airplane = await controller.PutAirplane(airplaneId, new Airplane());
            var objectResponse = airplane.Result as ObjectResult;
            Assert.AreEqual(objectResponse.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Category("PutAirplane")]
        [Test]
        public async Task ShouldReturnBadRequestWhenIdDoesNotMatch()
        {
            var airplaneId = 4;
            var airplane = new Airplane() { Id = 5, Code = "Updated1", Model = "Updated1", NumberOfSeats = 40 };
            var response = await controller.PutAirplane(airplaneId, airplane);
            var objectResponse = response.Result as StatusCodeResult;
            Assert.AreEqual(objectResponse.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [Category("DeleteAirplane")]
        [Test]
        public async Task ShoudlDeleteAirplane()
        {
            var airplaneId = 3;
            var response = await controller.DeleteAirplane(airplaneId);
            var objectResponse = response.Result as ObjectResult;
            Assert.AreEqual(objectResponse.StatusCode, (int)HttpStatusCode.OK);
        }

        [Category("DeleteAirplane")]
        [Test]
        public async Task ShouldReturnNotFoundWhenDeletingNonExistentAirplane()
        {
            var airplaneId = 300;
            var response = await controller.DeleteAirplane(airplaneId);
            var objectResponse = response.Result as NotFoundResult;
            Assert.AreEqual(objectResponse.StatusCode, (int)HttpStatusCode.NotFound);
        }

        private void AssertAirplane1(Airplane airplane)
        {
            Assert.AreEqual(airplane.Id, 1);
            Assert.AreEqual(airplane.Code, "A1");
            Assert.AreEqual(airplane.Model, "M1");
            Assert.AreEqual(airplane.NumberOfSeats, 180);
            Assert.AreEqual(airplane.CreationDate, DateTime.UtcNow);
        }
    }
}