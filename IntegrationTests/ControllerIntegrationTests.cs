﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SamuraiAPI.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ControllerIntegrationTests
    {
        private readonly WebApplicationFactory<SamuraiAPI.Startup> _factory;

        public ControllerIntegrationTests()
        {
            _factory = new WebApplicationFactory<SamuraiAPI.Startup>();
        }

        [TestMethod]
        public async Task GetEndpointReturnsSuccessAndSomeDataFromTheDatabase()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync("/api/SamuraisSoc");
            response.EnsureSuccessStatusCode(); // Status Code 200 - 299
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObjectList = JsonConvert.DeserializeObject<List<Samurai>>(responseString);
            // Assert
            Assert.AreNotEqual(0, responseObjectList.Count);
        }
    }
}
