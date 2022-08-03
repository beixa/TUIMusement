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
using TUIMusement.Weather;

namespace TUIMusement.Tests
{
    [TestFixture]
    public class WeatherServiceTests
    {
        private IHttpClientFactory HttpFactory;
        private IWeatherService WeatherForecast;
        private HttpClient Client;
        private MockHttpMessageHandler MockHttpMessageHandler;

        [SetUp]
        public void Setup()
        {
            HttpFactory = Substitute.For<IHttpClientFactory>();
            WeatherForecast = new WeatherService(HttpFactory);
            MockHttpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
            Client = new HttpClient(MockHttpMessageHandler);
        }

        [Test]
        public async Task ProcessCitiesWeather_ValidResponseFromClient_AsksForWeatherForecast()
        {
            //Arrange
            var cities = new List<City>() { new(1, "Test", 1, 1) };
            var content = new StringContent("{\"forecast\":{\"forecastday\":[{\"day\":{\"condition\":{\"text\":\"Sunny\"}}},{\"day\":{\"condition\":{\"text\":\"Sunny\"}}}]}}");
            var mockResponse = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = content };
            HttpFactory.CreateClient().Returns(Client);

            MockHttpMessageHandler.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
               .Returns(mockResponse);

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //Act
            await WeatherForecast.ProcessCitiesWeather(cities);

            //Assert
            Assert.AreEqual($"Processed city Test | Sunny - Sunny\r\n", stringWriter.ToString());
        }

        [Test]
        public async Task ProcessCitiesWeather_InValidResponseFromClient_AsksForWeatherForecast()
        {
            //Arrange
            var cities = new List<City>() { new(1, "Test", 1, 1) };
            var mockResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            HttpFactory.CreateClient().Returns(Client);

            MockHttpMessageHandler.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
               .Returns(mockResponse);

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //Act
            await WeatherForecast.ProcessCitiesWeather(cities);

            //Assert
            Assert.IsFalse(stringWriter.ToString().Contains("Processed"));
        }

        [Test]
        public async Task ProcessCitiesWeather_NullCities_DoesNotAskForWeatherForecast()
        {
            //Arrange;
            HttpFactory.CreateClient().Returns(Client);

            //Act
            await WeatherForecast.ProcessCitiesWeather(null);

            //Assert
            await MockHttpMessageHandler.Received(0).Send(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task ProcessCitiesWeather_EmptyCities_DoesNotAskForWeatherForecast()
        {
            //Arrange;
            var client = Substitute.For<HttpClient>();
            HttpFactory.CreateClient().Returns(client);

            //Act
            await WeatherForecast.ProcessCitiesWeather(null);

            //Assert
            await MockHttpMessageHandler.Received(0).Send(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>());
        }
    }
}