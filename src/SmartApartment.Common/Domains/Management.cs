namespace SmartApartment.Common.Domains
{
    using Nest;
    using Newtonsoft.Json;

    /// <summary>
    /// Represent a Management document.
    /// </summary>
    public class Management
    {
        /// <summary>
        /// For some reason sample data contain empty value for this field. therefore,
        /// to avoid unexpected behaviour, this field is set to be Nullable.
        /// </summary>
        [JsonProperty("mgmtID", NullValueHandling = NullValueHandling.Ignore)]
        public int? ManagementId { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}
