namespace SvcAsset.Core.Queries.Common
{
    public class PaginationParameters
    {
        private readonly int maxPageSize = 1000000;

        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
