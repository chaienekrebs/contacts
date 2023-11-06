using Newtonsoft.Json;

namespace Contacts.Domain.DTOs
{
    public class ApiResponse
    {
        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        [JsonProperty(PropertyName = "token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public static ApiResponse Success(object data = null, string token = "")
        {
            return new ApiResponse()
            {
                Status = "success",
                Data = data,
                Token = token
            };
        }
        public static ApiResponse Error(string message)
        {
            return new ApiResponse()
            {
                Status = "error",
                Message = message
            };
        }
    }
}