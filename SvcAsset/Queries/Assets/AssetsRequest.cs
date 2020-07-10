using MediatR;
using SvcAsset.Core.Models;
using SvcAsset.Core.Queries.Common;
using System;

namespace SvcAsset.Core.Queries.Assets
{
    public class AssetsRequest : PagedRequest, IRequest<PagedResponse<AssetModel, AssetModel>>
    {
        public Guid? ArticleId { get; }

        public string? Lmid { get; }

        public Guid TenantId { get; }

        public string? Purpose { get; }

        public AssetsRequest(PaginationParameters paginationParameters)
        {
            PaginationParameters = paginationParameters;
        }

        public AssetsRequest(
            PaginationParameters paginationParameters,
            Guid? articleId,
            string? lmid,
            Guid tenantId,
            string? purpose)
            : this(paginationParameters)
        {
            ArticleId = articleId;
            TenantId = tenantId;
            Lmid = lmid;
            Purpose = purpose;
        }
    }
}
