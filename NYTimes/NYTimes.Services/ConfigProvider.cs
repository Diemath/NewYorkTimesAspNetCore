using Services.Abstractions;
using Services.Abstractions.Configurations;

namespace Services.Concrete
{
  public class ConfigProvider : IConfigProvider
  {
    public ConfigProvider(AppConfig appConfig)
    {
      Config = appConfig;
    }

    public AppConfig Config { get; }
  }
}
