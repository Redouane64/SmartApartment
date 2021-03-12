namespace SmartApartment.Common.Abstraction
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using SmartApartment.Common.Domains;

    public interface ISearchService
    {
        Task<IEnumerable<Document>> Search(
            string keyword, 
            string market = null, 
            int limit = 25, 
            CancellationToken cancellationToken = default);
    }
}
