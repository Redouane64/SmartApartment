namespace SmartApartment.Common.Services
{
    using System;
    using System.Threading.Tasks;
    using Nest;
    using SmartApartment.Common.Abstraction;
    using SmartApartment.Common.Domains;

    public class SearchService : ISearchService
    {
        private readonly IElasticClient client;

        public SearchService(IElasticClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Task<Document> Search(string keyword, string market = null, int limit = 25)
        {
            throw new System.NotImplementedException();
        }
    }
}
