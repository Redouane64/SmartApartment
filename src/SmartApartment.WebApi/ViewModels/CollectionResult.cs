namespace SmartApartment.WebApi.ViewModels
{
    using System.Collections.Generic;

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
