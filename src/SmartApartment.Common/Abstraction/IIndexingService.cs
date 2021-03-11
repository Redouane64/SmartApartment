namespace SmartApartment.Common.Abstraction
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IIndexingService<TDocument> where TDocument : class
    {
        Task CreateIndex(string name, CancellationToken cancellationToken = default);
        Task IndexDocument(TDocument document, CancellationToken cancellationToken = default);
    }
}
