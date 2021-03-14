namespace SmartApartment.Common.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstraction;
    using Nest;

    using SmartApartment.Common.Domains;

    public sealed class IndexingService : IIndexingService<Document>
    {
        private readonly IElasticClient elasticClient;

        public IndexingService(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        }

        // Create or update index for Document type with mapping.
        public async Task<CreateIndexResponse> CreateIndex(string name, CancellationToken cancellationToken = default)
        {
            var indexConfig = new Func<IndexSettingsDescriptor, IPromise<IIndexSettings>>(
                settings => settings.Analysis(
                    // add english stop word analyzer in order to 
                    // skip this words during indexing.
                    a => a.Analyzers(
                        analyzers => analyzers.Standard(
                            "standard_english",
                            desc => desc.StopWords("_english_")
                        )
                    )
                    // add stop words filter.
                    .TokenFilters(
                        filter => filter.Stop("stop", desc => desc.StopWords("_english_"))
                    )
                ));

            return await this.elasticClient.Indices.CreateAsync(
                name,
                descriptor => descriptor.Map(
                    mappingDescriptor =>
                        mappingDescriptor
                            .AutoMap<Document>()
                    ).Settings(indexConfig),
                cancellationToken
            );
        }

        // Index new Document object.
        public async Task<IndexResponse> IndexDocument(Document document, CancellationToken cancellationToken = default)
        {
            return await this.elasticClient.IndexDocumentAsync(document, cancellationToken);
        }
    }
}
