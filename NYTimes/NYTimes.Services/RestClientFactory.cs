using NYTimes.Services.Abstractions;
using RestSharp;

namespace NYTimes.Services
{
    public class RestClientFactory : IRestClientFactory
    {
        public IRestClient GetRestClient()
        {
            return new RestClient();
        }
    }
}
