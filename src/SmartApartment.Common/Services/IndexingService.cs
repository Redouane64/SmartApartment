namespace SmartApartment.Common.Services
{
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
            this.elasticClient = elasticClient;
        }

        // Create or update index for Document type with mapping.
        public async Task<CreateIndexResponse> CreateIndex(string name, CancellationToken cancellationToken = default)
        {
            return await this.elasticClient.Indices.CreateAsync(
                name,
                descriptor => descriptor.Map(
                    mappingDescriptor =>
                        mappingDescriptor
                            .AutoMap<Document>()
                    ),
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
