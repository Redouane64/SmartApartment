namespace SmartApartment.Common.Domains
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represent a Property root object as defined in JSON sample data.
    /// </summary>
    public class PropertyRoot
    {
        public int? Id
        {
            get
            {
                // Use Id of child document.
                return this.Property?.PropertyId;
            }
        }

        [JsonProperty("property")]
        public Property Property { get; set; }
    }
}
