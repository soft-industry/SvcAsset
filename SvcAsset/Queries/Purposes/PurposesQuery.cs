using MediatR;
using Microsoft.EntityFrameworkCore;
using SvcAsset.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Queries.Purposes
{
    public class PurposesQuery : IRequestHandler<PurposesRequest, List<string>>
    {
        private readonly IEFContext _ctx;

        public PurposesQuery(IEFContext ctx)
        {
            _ctx = ctx;
        }

        public Task<List<string>> Handle(PurposesRequest request, CancellationToken cancellationToken)
        {
            return _ctx.Assets
                .Where(x => x.HolderTenantId == request.TenantId && x.ArticleId == request.ArticleId)
                .Include(x => x.Purposes)
                .SelectMany(x => x.Purposes)
                .Select(x => x.PurposeId.ToString())
                .Distinct()
                .ToListAsync();
        }
    }
}
