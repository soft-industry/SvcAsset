using Ardalis.GuardClauses;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SvcAsset.Core.Helpers
{
    public static class IQueryableExtensions
    {
        private static readonly string Descending = "DESC";

        private static readonly string Ascending = "ASC";

        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> query,
            IDictionary<string, bool> sortParams)
        {
            Guard.Against.Null(query, nameof(query));
            Guard.Against.Null(sortParams, nameof(sortParams));

            if (sortParams.Count == 0)
            {
                return query;
            }

            var isFirstSortParam = true;

            //assure the variable won't be null when returned
            IOrderedQueryable<T> orderQuery = null!;

            foreach (var sortParam in sortParams)
            {
                var sortDirection = $"{sortParam.Key} {(sortParam.Value ? Ascending : Descending)}";

                if (isFirstSortParam)
                {
                    orderQuery = query.OrderBy(sortDirection);

                    isFirstSortParam = false;

                    continue;
                }

                orderQuery = orderQuery.ThenBy(sortDirection);
            }

            return orderQuery;
        }
    }
}
