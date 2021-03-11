namespace SmartApartment.Common.Abstraction
{
    using System.Threading;
    using System.Threading.Tasks;
    
    using Nest;

    public interface IIndexingService<TDocument> where TDocument : class
    {
        Task<CreateIndexResponse> CreateIndex(string name, CancellationToken cancellationToken = default);
        Task<IndexResponse> IndexDocument(TDocument document, CancellationToken cancellationToken = default);
    }
}
