using Newtonsoft.Json;
using Utils;

namespace Model
{
    public class LevelDifficult
    {
        [JsonProperty("id")]
        public int LevelId { get; set; }
        
        [JsonProperty("ast")]
        public int AsteroidsCount { get; set; }
        
        [JsonProperty("ufo_delay")]
        public int UfoDelay { get; set; }
    }
}