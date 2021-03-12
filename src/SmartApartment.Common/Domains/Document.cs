namespace SmartApartment.Common.Domains
{

    using Newtonsoft.Json;

    public class Document
    {
        // Id for elasticsearch indexing
        public int? Id
        {
            get
            {
                // Use Id of child managment or property document, otherwise null.
                return Property?.PropertyId ?? Management?.ManagementId ?? null;
            }
        }

        [JsonProperty("property")]
        public Property Property { get; set; }

        [JsonProperty("mgmt")]
        public Management Management { get; set; }
    }
}
