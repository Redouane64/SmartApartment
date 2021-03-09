namespace SampleDataUploader
{
    using Newtonsoft.Json;

    using SmartApartment.Common.Domains;

    public class PropertyRoot
    {
        [JsonProperty("property")]
        public Property Property { get; set; }
    }

}
