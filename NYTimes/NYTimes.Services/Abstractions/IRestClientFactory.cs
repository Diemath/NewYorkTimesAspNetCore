using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NYTimes.Services.Abstractions
{
  public interface IRestClientFactory
  {
    IRestClient GetRestClient();
  }
}
