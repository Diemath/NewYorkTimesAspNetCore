namespace NYTimes.Services.Configurations
{
    public class ApiOptions
    {
        public string BaseUrl { get; set; }
        public string Resource { get; set; }
        public string Key { get; set; }
        public string ShortUrlTemplate { get; set; }
        public HeaderOptions Headers { get; set; }
    }
}
