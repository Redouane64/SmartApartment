using System.Collections.Generic;

namespace SmartApartment.WebApi.ViewModels
{
    public class CollectionResult<T>
        where T : class
    {
        public int Size { get; }

        public IEnumerable<T> Items { get; }

        public CollectionResult(int size, IEnumerable<T> items)
        {
            Size = size;
            Items = items;
        }
    }
}
