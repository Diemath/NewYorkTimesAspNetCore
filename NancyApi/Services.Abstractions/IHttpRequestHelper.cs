using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Abstractions
{
  public interface IHttpRequestHelper
  {
    Task<string> GetAsync(string endpoint, params KeyValuePair<string, string>[] requestQuery);
  }
}
