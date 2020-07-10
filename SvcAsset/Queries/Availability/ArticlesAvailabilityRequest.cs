using MediatR;
using SvcAsset.Core.Models;
using SvcAsset.Core.Queries.Parameters;
using System;

namespace SvcAsset.Core.Queries.Availability
{
    public class ArticlesAvailabilityRequest : IRequest<AvailabilitiesModel>
    {
        public AvailableArticlesModel AvailableArticlesModel { get; set; }

        public AvailabilityQueryParamaters AvailabilityQueryParamaters { get; set; }

        public Guid TenantId { get; set; }

        public ArticlesAvailabilityRequest(AvailableArticlesModel availableArticlesModel, AvailabilityQueryParamaters availabilityQueryParamaters, Guid tenatnId)
        {
            AvailableArticlesModel = availableArticlesModel;
            AvailabilityQueryParamaters = availabilityQueryParamaters;
            TenantId = tenatnId;
        }
    }
}
