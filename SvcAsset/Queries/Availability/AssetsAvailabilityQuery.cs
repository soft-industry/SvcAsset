using MediatR;
using Microsoft.EntityFrameworkCore;
using SvcAsset.Core.Helpers;
using SvcAsset.Core.Interfaces;
using SvcAsset.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Queries.Availability
{
    public class AssetsAvailabilityQuery : IRequestHandler<AssetsAvailabilityRequest, AvailableItemsModel>
    {
        private readonly IEFContext _ctx;

        public AssetsAvailabilityQuery(IEFContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<AvailableItemsModel> Handle(AssetsAvailabilityRequest request, CancellationToken cancellationToken)
        {
            var skipEvents = await _ctx.Events.Where(x => request.AvailableAssetsModel.SkipEventIds.Contains(x.Id)).ToListAsync(cancellationToken);

            if (skipEvents.Any())
            {
                skipEvents.ForEach(x => x.SoftDelete());
                await _ctx.SaveChangesAsync(cancellationToken);
            }

            var availableIds = await GetAvailableAssets(request, cancellationToken);

            if (skipEvents.Any())
            {
                skipEvents.ForEach(x => x.SoftRestore());
                await _ctx.SaveChangesAsync(cancellationToken);
            }

            return new AvailableItemsModel
            {
                AvailableIds = availableIds
            };
        }

        private async Task<List<Guid>> GetAvailableAssets(AssetsAvailabilityRequest request, CancellationToken cancellationToken)
        {
            var minutesInterval = DateTimeUtils.GetIntervalForUnit(request.AvailabilityQueryParamaters.Unit, request.AvailabilityQueryParamaters.Number);

            var availableAssets = await _ctx.EventGaps.Where(x => x.TenantId == request.TenantId
                    && x.StartGap <= request.AvailabilityQueryParamaters.End
                    && x.EndGap >= request.AvailabilityQueryParamaters.Start
                    && request.AvailableAssetsModel.AssetIds.Contains(x.AssetId)).ToListAsync(cancellationToken);

            if (request.AvailabilityQueryParamaters.Number > 0)
            {
                var interval1 = new DateTimeInterval(request.AvailabilityQueryParamaters.Start, request.AvailabilityQueryParamaters.End);

                for (int i = availableAssets.Count - 1; i >= 0; i--)
                {
                    if (availableAssets[i].AvailabilityDate != null && availableAssets[i].StartGap <= availableAssets[i].AvailabilityDate)
                    {
#pragma warning disable CS8629 // Nullable value type may be null.
                        availableAssets[i].StartGap = availableAssets[i].AvailabilityDate.Value;
#pragma warning restore CS8629 // Nullable value type may be null.
                    }

                    var interval2 = new DateTimeInterval(availableAssets[i].StartGap, availableAssets[i].EndGap);
                    var minutesIntersection = DateTimeUtils.GetIntervalIntersection(interval1, interval2);
                    if (minutesIntersection < minutesInterval)
                    {
                        availableAssets.RemoveAt(i);
                    }
                }
            }

            return availableAssets.Select(x => x.AssetId).Distinct().ToList();
        }
    }
}
