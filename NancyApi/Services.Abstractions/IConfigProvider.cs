using Services.Abstractions.Configurations;

namespace Services.Abstractions
{
  public interface IConfigProvider
  {
    AppConfig Config { get; }
  }
}
