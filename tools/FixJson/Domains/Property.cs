using Newtonsoft.Json;

using System;

namespace FixJson
{
    [Serializable]
    public class Property
    {
        public int propertyID { get; set; }
        public string name { get; set; }
        public string formerName { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string market { get; set; }
        public string state { get; set; }

        [JsonIgnore]
        public double lat { get; set; }

        [JsonIgnore]
        public double lng { get; set; }
    }

}
