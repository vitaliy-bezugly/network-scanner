using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IpScanner.Helpers.Filters
{
    public class FilteredCollection<T> : ObservableCollection<T>
    {
        private readonly ObservableCollection<T> filteredItems;
        private readonly ICollection<ItemFilter<T>> filters;

        public FilteredCollection() : base()
        {
            filteredItems = new ObservableCollection<T>();
            filters = new List<ItemFilter<T>>();
        }

        public ObservableCollection<T> FilteredItems
        {
            get => filteredItems;
        }

        public void AddFilter(ItemFilter<T> filter)
        {
            filters.Add(filter);
        }

        public void RemoveFilter(ItemFilter<T> filter)
        {
            filters.Remove(filter);
        }

        public void RefreshFilteredItems()
        {
            FilteredItems.Clear();

            foreach (var item in this)
            {
                if (ItemSutisfiesFilters(item))
                {
                    FilteredItems.Add(item);
                }
            }
        }

        public void ReplaceItem(T oldItem, T newItem)
        {
            var index = IndexOf(oldItem);

            if (index != -1)
            {
                Remove(oldItem);
                Insert(index, newItem);
            }
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            if (ItemSutisfiesFilters(item))
            {
                if(index >= filteredItems.Count)
                {
                    filteredItems.Add(item);
                }
                else
                {
                    filteredItems.Insert(index, item);
                }
            }
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            if(index < filteredItems.Count)
            {
                filteredItems.RemoveAt(index);
            }
        }

        protected override void ClearItems()
        {
            filteredItems.Clear();
            base.ClearItems();
        }

        private bool ItemSutisfiesFilters(T item)
        {
            foreach (var filter in filters)
            {
                if (filter.Filter.Invoke(item) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
