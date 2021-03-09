namespace SampleDataUploader
{
    using Newtonsoft.Json;

    using SmartApartment.Common.Domains;

    public class ManagementRoot
    {
        [JsonProperty("mgmt")]
        public Management Management { get; set; }
    }

}
