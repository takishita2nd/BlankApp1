using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlankApp1
{
    [JsonObject("sensorModel")]
    class Sensor
    {
        [JsonProperty("temperature")]
        public int Temperature { get; set; }
        [JsonProperty("humidity")]
        public int Humidity { get; set; }
        [JsonProperty("pressure")]
        public int Pressure { get; set; }
    }
}

