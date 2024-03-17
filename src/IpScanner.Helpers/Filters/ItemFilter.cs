using System;

namespace IpScanner.Helpers.Filters
{
    public class ItemFilter<T> : IEquatable<ItemFilter<T>>
    {
        private readonly Guid id;

        public ItemFilter(Func<T, bool> filter)
        {
            id = Guid.NewGuid();
            Filter = filter;
        }

        public Func<T, bool> Filter { get; }

        public bool Equals(ItemFilter<T> other)
        {
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            return obj as ItemFilter<T> != null && Equals(obj as ItemFilter<T>); 
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
