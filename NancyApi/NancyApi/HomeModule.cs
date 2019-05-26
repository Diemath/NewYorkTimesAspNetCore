using Nancy;

namespace NancyApi
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get("/", _ => {
        return "Hello World!!!";
      });
      Get("/list/{section}", _ => {
        return $"section: {_.section}";
      });
      Get("/list/{section}/first", _ => {
        return $"section: {_.section}";
      });
      Get("/list/{section}/{updatedDate}", _ => {
        return $"section: {_.section}; updatedDate: {_.updatedDate}";
      });
      Get("/article/{shortUrl}", _ => {
        return $"shortUrl: {_.shortUrl}";
      });
      Get("/group/{section}", _ => {
        return $"section: {_.section}";
      });
    }
  }
}
