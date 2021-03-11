namespace SmartApartment.Common.Abstraction
{
    using System.Threading.Tasks;
    using SmartApartment.Common.Domains;

    public interface ISearchService
    {
        Task<Document> Search(string keyword, string market = null, int limit = 25);
    }
}
