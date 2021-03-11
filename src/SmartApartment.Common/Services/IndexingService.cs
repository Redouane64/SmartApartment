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
        public async Task CreateIndex(string name, CancellationToken cancellationToken = default)
        {
            await this.elasticClient.Indices.CreateAsync(
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
        public async Task IndexDocument(Document document, CancellationToken cancellationToken = default)
        {
            await this.elasticClient.IndexDocumentAsync(document, cancellationToken);
        }
    }
}
