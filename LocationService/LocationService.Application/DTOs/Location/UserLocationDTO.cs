namespace LocationService.Location.Application.DTOs
{
    public class UserLocationDTO
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string ip_address { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}
