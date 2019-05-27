using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Abstractions.Configurations
{
  public class AppConfig
  {
    public NyTimesApiConfig NyTimesApi { get; set; } = new NyTimesApiConfig();
  }
}
