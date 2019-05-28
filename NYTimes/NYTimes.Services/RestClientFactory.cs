using NYTimes.Services.Abstractions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

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
