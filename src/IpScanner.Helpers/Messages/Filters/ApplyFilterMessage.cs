using IpScanner.Helpers.Filters;

namespace IpScanner.Helpers.Messages.Filters
{
    public class ApplyFilterMessage<T>
    {
        public ApplyFilterMessage(ItemFilter<T> filter, bool filterStatus)
        {
            Filter = filter;
            FilterStatus = filterStatus;
        }

        public ItemFilter<T> Filter { get; }
        public bool FilterStatus { get; }
    }
}
