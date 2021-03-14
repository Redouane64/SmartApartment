namespace SmartApartment.WebApi.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class SearchOptions
    {
        public const int DefaultLimit = 25;

        [Required]
        public string Keyword { get; set; }

        /// <summary>
        /// Markets scope separated by comma (,)
        /// </summary>
        /// <example>Atlanta, Sacramento</example>
        public string Markets { get; set; }

        [Range(0, 999)]
        public int Limit { get; set; } = DefaultLimit;
    }
}
