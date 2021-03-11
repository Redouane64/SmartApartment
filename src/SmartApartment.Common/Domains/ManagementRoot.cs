namespace SmartApartment.Common.Domains
{
    using Nest;

    using Newtonsoft.Json;

    public class ManagementRoot
    {
        public int? Id { 
            get {
                // Use Id of child document.
                return this.Management?.ManagementId;
            } 
        }

        [JsonProperty("mgmt")]
        public Management Management { get; set; }
    }
}
