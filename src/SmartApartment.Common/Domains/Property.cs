namespace SmartApartment.Common.Domains
{
    using Nest;
    using Newtonsoft.Json;

    /// <summary>
    /// Represent a Property document.
    /// </summary>
    [ElasticsearchType(IdProperty = nameof(Property.PropertyId))]
    public class Property
    {
        /// <summary>
        /// For some reason sample data contain empty value for this field. therefore,
        /// to avoid unexpected behaviour, this field is set to be Nullable.
        /// </summary>
        [JsonProperty("propertyID", NullValueHandling = NullValueHandling.Ignore)]
        public int? PropertyId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("formerName")]
        public string FormerName { get; set; }

        [JsonProperty("streetAddress")]
        public string StreetAddress { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public double? Latitude { get; set; }

        [JsonProperty("lng", NullValueHandling = NullValueHandling.Ignore)]
        public double? Longitude { get; set; }
    }

}
