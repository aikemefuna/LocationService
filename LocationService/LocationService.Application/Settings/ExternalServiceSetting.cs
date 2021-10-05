namespace LocationService.Application.Settings
{
    public class ExternalServiceSetting
    {
        public string BaseUrl { get; set; }
        public string GetAllUsers { get; set; }
        public string GetUsersByCity { get; set; }
        public string GetUserById { get; set; }
        public double LongitudeOfLondon { get; set; }
        public double LatitudeOfLondon { get; set; }
        public double MilesAverage { get; set; }
        public string Location { get; set; }
    }
}
