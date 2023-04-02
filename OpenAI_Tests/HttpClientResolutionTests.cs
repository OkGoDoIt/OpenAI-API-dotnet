using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OpenAI_API;
using System;
using System.Linq;
using System.Net.Http;

namespace OpenAI_Tests
{
    public class HttpClientResolutionTests
    {
        [Test]
        public void GetHttpClient_NoFactory()
        {
            var api = new OpenAIAPI(new APIAuthentication("fake-key"));
            var endpoint = new TestEndpoint(api);

            var client = endpoint.GetHttpClient();
            Assert.IsNotNull(client);
        }

        [Test]
        public void GetHttpClient_WithFactory()
        {
            var expectedClient1 = new HttpClient();
            var mockedFactory1 = Mock.Of<IHttpClientFactory>(f => f.CreateClient(Options.DefaultName) == expectedClient1);

            var expectedClient2 = new HttpClient();
            var mockedFactory2 = Mock.Of<IHttpClientFactory>(f => f.CreateClient(Options.DefaultName) == expectedClient2);

            var api = new OpenAIAPI(new APIAuthentication("fake-key"));
            var endpoint = new TestEndpoint(api);

            api.HttpClientFactory = mockedFactory1;
            var actualClient1 = endpoint.GetHttpClient();

            api.HttpClientFactory = mockedFactory2;
            var actualClient2 = endpoint.GetHttpClient();

            Assert.AreSame(expectedClient1, actualClient1);
            Assert.AreSame(expectedClient2, actualClient2);

            api.HttpClientFactory = null;
            var actualClient3 = endpoint.GetHttpClient();

            Assert.NotNull(actualClient3);
            Assert.AreNotSame(expectedClient1, actualClient3);
            Assert.AreNotSame(expectedClient2, actualClient3);
        }

        private class TestEndpoint : EndpointBase
        {
            public TestEndpoint(OpenAIAPI api) : base(api)
            {
            }

            protected override string Endpoint => throw new System.NotSupportedException();

            public HttpClient GetHttpClient()
            {
                return base.GetClient();
            }
        }
    }
}
