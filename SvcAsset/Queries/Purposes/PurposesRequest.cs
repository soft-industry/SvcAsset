using MediatR;
using System;
using System.Collections.Generic;

namespace SvcAsset.Core.Queries.Purposes
{
    public class PurposesRequest : IRequest<List<string>>
    {
        public Guid ArticleId { get; set; }

        public Guid TenantId { get; set; }

        public PurposesRequest(Guid articleId, Guid tenantId)
        {
            ArticleId = articleId;
            TenantId = tenantId;
        }
    }
}
