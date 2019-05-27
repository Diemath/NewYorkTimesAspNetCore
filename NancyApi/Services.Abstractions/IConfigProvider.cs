using Services.Abstractions.Configurations;
using Services.Abstractions.Dto;
using Services.Abstractions.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Abstractions
{
  public interface IConfigProvider
  {
    AppConfig Config { get; }
  }
}
