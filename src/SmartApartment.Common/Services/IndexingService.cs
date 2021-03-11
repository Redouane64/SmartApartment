namespace SmartApartment.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstraction;
    using Nest;

    using SmartApartment.Common.Domains;

    public sealed class IndexingService : IIndexingService
    {
        private readonly IElasticClient elasticClient;

        public IndexingService(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task CreateIndex(string name, CancellationToken cancellationToken = default)
        {
            await this.elasticClient.Indices.CreateAsync(
                name, 
                descriptor =>  descriptor.Map(
                    mappingDescriptor => 
                        mappingDescriptor
                            .AutoMap<Document>()
                    ), 
                cancellationToken
            );
        }

        public async Task<IndexResponse> IndexDocument<TDocument>(
            TDocument document, 
            CancellationToken cancellationToken = default)
            where TDocument : class
        {
            return await this.elasticClient.IndexDocumentAsync(document, cancellationToken);
        }
    }
}
