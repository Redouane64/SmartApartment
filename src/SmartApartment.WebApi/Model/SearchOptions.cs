namespace SmartApartment.WebApi.Model
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class SearchOptions
    {
        public const int DefaultLimit = 25;

        [Required]
        public string Keyword { get; set; }

        public string Market { get; set; }

        [DefaultValue(DefaultLimit)]
        public int Limit { get; set; }
    }
}
