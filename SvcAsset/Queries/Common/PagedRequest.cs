namespace SvcAsset.Core.Queries.Common
{
    public class PagedRequest
    {
        public PaginationParameters PaginationParameters { get; protected set; }

        public PagedRequest()
        {
            PaginationParameters = new PaginationParameters();
        }
    }
}
