using Newtonsoft.Json;

namespace Geta.SocialChannels.Facebook.DTO
{
    public class AccountInformationDto
    {
        [JsonProperty("id")] 
        public string Id { get; set; }
        
        [JsonProperty("about")] 
        public string About { get; set; }
        
        [JsonProperty("description")] 
        public string Description { get; set; }
        
        [JsonProperty("phone")] 
        public string Phone { get; set; }
        
        [JsonProperty("website")] 
        public string Website { get; set; }
        
        [JsonProperty("location")] 
        public LocationDto Location { get; set; }
        
        [JsonProperty("username")] 
        public string Username { get; set; }
    }

    public class LocationDto
    {
        [JsonProperty("city")] 
        public string City { get; set; }
        
        [JsonProperty("country")] 
        public string Country { get; set; }
        
        [JsonProperty("latitude")] 
        public double Latitude { get; set; }
        
        [JsonProperty("longitude")] 
        public double Longitude { get; set; }
        
        [JsonProperty("street")] 
        public string Street { get; set; }
        
        [JsonProperty("zip")] 
        public string Zip { get; set; }
    }
}