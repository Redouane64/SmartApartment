using System.Text.Json.Serialization;
using SmartApartment.Common.Domains;

namespace SmartApartment.WebApi.ViewModels
{
    public class DocumentViewModel
    {
        [JsonPropertyName("mgmt")]
        public Management Management { get; set; }

        [JsonPropertyName("property")]
        public Property Property { get; set; }
    }
}
