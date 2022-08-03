using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TUIMusement.Cities;

namespace TUIMusement.Tests
{
    [TestFixture]
    public class CitiesServiceTests
    {
        private IHttpClientFactory HttpFactory;
        private ICitiesService CitiesService;
        private HttpClient Client;
        private MockHttpMessageHandler MockHttpMessageHandler;

        [SetUp]
        public void Setup()
        {
            HttpFactory = Substitute.For<IHttpClientFactory>();
            CitiesService = new CitiesService(HttpFactory);

            MockHttpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
            Client = new HttpClient(MockHttpMessageHandler);
        }

        [Test]
        public async Task GetCities_ValidResponseFromClient_AsksForCitiesToClient()
        {
            //Arrange
            var mockResponse = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent("[{\"id\":57,\"name\":\"Amsterdam\",\"latitude\":52.374,\"longitude\":4.9}]") };
            var expectedResponse = new List<City> { new City(57, "Amsterdam", (float)52.374, (float)4.9) };

            HttpFactory.CreateClient().Returns(Client);

            MockHttpMessageHandler.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
               .Returns(mockResponse);

            //Act
            var result = await CitiesService.GetCities();

            //Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, expectedResponse);
        }

        [Test]
        public async Task GetCities_InValidResponseFromClient_DoesNotAsksForCitiesToClient()
        {
            //Arrange
            var mockResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            HttpFactory.CreateClient().Returns(Client);

            MockHttpMessageHandler.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
               .Returns(mockResponse);

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //Act
            var result = await CitiesService.GetCities();

            //Assert
            Assert.IsNull(result);
            Assert.IsFalse(stringWriter.ToString().Contains("Processed"));
        }
    }
}
