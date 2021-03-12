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

        // define searchable fields in Document type.
        // it is preferrable to me to be more explicit in my code.
        private readonly Func<FieldsDescriptor<Document>, IPromise<Fields>> searchableFields =
            f => f.Field(e => e.Property.Name)
                            .Field(e => e.Management.Name)
                            .Field(e => e.Property.FormerName)
                            .Field(e => e.Property.StreetAddress)
                            .Field(e => e.Property.State)
                            .Field(e => e.Management.State);
        private readonly IElasticClient client;

        public SearchService(IElasticClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<Document>> Search(
            string keyword,
            string markets = null,
            int limit = 25,
            CancellationToken cancellationToken = default)
        {
            // define query function for our keyword with list of selected fields from
            // predefined `searchField` variable.
            var keywordQuery = new Func<QueryContainerDescriptor<Document>, QueryContainer>(
                q => q.SimpleQueryString(
                    s => s.Fields(searchableFields).Query(keyword)
                )
            );

            // construct search descriptor.
            Func<BoolQueryDescriptor<Document>, IBoolQuery> query = null;

            // query with markets list provided.
            if (markets is not null)
            {
                query = new Func<BoolQueryDescriptor<Document>, IBoolQuery>(
                    b => b.Should(keywordQuery)
                          .Must(t => t.Bool(b => b.Should(toMarketsQueries(markets.Split(",")))))
                );
            } else
            // query without market list
            {
                query = new Func<BoolQueryDescriptor<Document>, IBoolQuery>(
                    b => b.Should(keywordQuery)
                );
            }

            var selector = new Func<SearchDescriptor<Document>, ISearchRequest>(
                s => s.Size(limit).Query(
                    q => q.Bool(query)
                )
            );

            // execute search request.
            var response = await this.client.SearchAsync<Document>(selector, cancellationToken);

            if (!response.IsValid)
            {
                throw new Exception("Unable to process search operation.");
            }

            return response.Documents;
        }

        // convert market terms to elasticsearch queries.
        private IEnumerable<Func<QueryContainerDescriptor<Document>, QueryContainer>> toMarketsQueries(string[] markets)
        {
            foreach (var market in markets)
            {
                yield return new Func<QueryContainerDescriptor<Document>, QueryContainer>(
                    q => q.MultiMatch(
                        f => f.Fields(
                            m => m.Field(e => e.Property.Market)
                                .Field(e => e.Management.Market)
                        ).Query(market)
                    )
                );
            }
        }
    }
}
