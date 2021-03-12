namespace SmartApartment.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
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

        public async Task<IEnumerable<Document>> Search(string keyword,
                                                  string market = null,
                                                  int limit = 25,
                                                  CancellationToken cancellationToken = default)
        {
            var response = await this.client.SearchAsync<Document>(
                s => s.Size(limit)
                      .QueryOnQueryString(keyword)
            , cancellationToken);

            if (!response.IsValid)
            {
                throw new Exception("Unable to process search operation.");
            }

            return response.Documents;
        }
    }
}
