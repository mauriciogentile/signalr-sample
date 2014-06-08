using Newtonsoft.Json;

namespace Solaise.Weather.Web.Resources
{
    public abstract class Resource
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}