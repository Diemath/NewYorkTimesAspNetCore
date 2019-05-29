using RestSharp;

namespace NYTimes.Services.Abstractions
{
    public interface IRestClientFactory
    {
        IRestClient GetRestClient();
    }
}
