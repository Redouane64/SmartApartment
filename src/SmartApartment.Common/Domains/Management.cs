namespace SmartApartment.Common.Domains
{
    using System;

    using Newtonsoft.Json;

    [Serializable]
    public class Management
    {
        [JsonProperty("mgmtID")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }

}
