using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Abstractions
{
  public interface IHttpGetter
  {
    Task<string> GetAsync(string url, params KeyValuePair<string, string>[] parameters);
  }
}
