using Services.Abstractions;
using Services.Abstractions.Configurations;

namespace Services.Concrete
{
  public class ConfigProvider : IConfigProvider
  {
    private readonly AppConfig _appConfig;

    public ConfigProvider(AppConfig appConfig)
    {
      _appConfig = appConfig;
    }

    public AppConfig Config => _appConfig;
  }
}
