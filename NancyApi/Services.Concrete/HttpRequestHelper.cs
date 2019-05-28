using Services.Abstractions;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services.Concrete
{
  public class HttpRequestHelper : IHttpRequestHelper
  {
    public async Task<string> GetAsync(string endpoint, params KeyValuePair<string, string>[] requestQuery)
    {
      string responseMessage;
      var sb = new StringBuilder(endpoint);

      if (requestQuery.Any())
      {
        NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);

        foreach (KeyValuePair<string, string> keyValuePair in requestQuery)
          nameValueCollection[keyValuePair.Key] = keyValuePair.Value;
        
        sb.Append("?")
          .Append(nameValueCollection.ToString());
      }

      using (var client = new HttpClient())
        responseMessage = await (await client.GetAsync(sb.ToString())).Content.ReadAsStringAsync();

      return responseMessage;
    }
  }
}
