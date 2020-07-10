using MediatR;
using Microsoft.EntityFrameworkCore;
using SvcAsset.Core.Helpers;
using SvcAsset.Core.Interfaces;
using SvcAsset.Core.Models;
using SvcAsset.Core.Queries.Common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Queries.Assets
{
    public class AssetsQuery : IRequestHandler<AssetsRequest, PagedResponse<AssetModel, AssetModel>>
    {
        private readonly Interfaces.IEFContext _ctx;

        public AssetsQuery(IEFContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<PagedResponse<AssetModel, AssetModel>> Handle(
            AssetsRequest request,
            CancellationToken cancellationToken)
        {
            var items = _ctx.Assets.AsQueryable();

            if (request.ArticleId != null)
            {
                items = items.Where(x => x.ArticleId == request.ArticleId);
            }

            if (!string.IsNullOrEmpty(request.Lmid))
            {
                items = items.Where(x => x.LMID == request.Lmid);
            }
            else
            {
                items = items.Where(x => x.HolderTenantId == request.TenantId);
            }

            if (!string.IsNullOrEmpty(request.Purpose))
            {
                items = items
                    .Where(x => x.Purposes.Select(x => x.PurposeId.ToString()).Contains(request.Purpose));
            }

            var result = items.GetAssetModels().OrderBy(x => x.Lmid).AsNoTracking();

            return await PagedResponse<AssetModel, AssetModel>.Create(
                item => item,
                result,
                request.PaginationParameters.PageNumber,
                request.PaginationParameters.PageSize,
                cancellationToken);
        }
    }
}
