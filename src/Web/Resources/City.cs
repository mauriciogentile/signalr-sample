using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Solaise.Weather.Web.Resources
{
    public class City : Resource
    {
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [Required]
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
    }
}