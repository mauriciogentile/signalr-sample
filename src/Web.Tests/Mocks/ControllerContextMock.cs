using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Hosting;
using Moq;

namespace Solaise.Weather.Web.Tests.Mocks
{
    public static class MocksUtils
    {
        public static HttpRequestMessage MockRequest(HttpRequestHeaders headers = null)
        {
            var request = new HttpRequestMessage();
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            return request;
        }
    }
}
