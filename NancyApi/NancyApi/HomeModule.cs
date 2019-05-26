using Nancy;

namespace NancyApi
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get("/", _ => "Hello World!!!");
      Get("/about", _ => "Brought to you buy FancyFX");
    }
  }
}
