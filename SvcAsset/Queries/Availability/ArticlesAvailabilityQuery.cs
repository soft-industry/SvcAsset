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
    public class ArticlesAvailabilityQuery : IRequestHandler<ArticlesAvailabilityRequest, AvailabilitiesModel>
    {
        private readonly IEFContext _ctx;

        public ArticlesAvailabilityQuery(IEFContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<AvailabilitiesModel> Handle(ArticlesAvailabilityRequest request, CancellationToken cancellationToken)
        {
            var skipEvents = await _ctx.Events.Where(x => request.AvailableArticlesModel.SkipEventIds.Contains(x.Id)).ToListAsync(cancellationToken);

            if (skipEvents.Any())
            {
                skipEvents.ForEach(x => x.SoftDelete());

                await _ctx.SaveChangesAsync(cancellationToken);
            }

            var availabilities = await GetAvailableArticles(request, cancellationToken);

            if (skipEvents.Any())
            {
                skipEvents.ForEach(x => x.SoftRestore());
                await _ctx.SaveChangesAsync(cancellationToken);
            }

            return new AvailabilitiesModel
            {
                Availabilities = availabilities
            };
        }

        private async Task<IEnumerable<AvailabilityModel>> GetAvailableArticles(ArticlesAvailabilityRequest request, CancellationToken cancellationToken)
        {
            var minutesInterval = DateTimeUtils.GetIntervalForUnit(request.AvailabilityQueryParamaters.Unit, request.AvailabilityQueryParamaters.Number);

            var availableArticles = await _ctx.EventGaps.Where(x => x.TenantId == request.TenantId
                      && x.StartGap <= request.AvailabilityQueryParamaters.End
                      && x.EndGap >= request.AvailabilityQueryParamaters.Start
                      && request.AvailableArticlesModel.ArticleIds.Contains(x.ArticleId)).ToListAsync(cancellationToken);

            if (request.AvailabilityQueryParamaters.Number > 0)
            {
                var interval1 = new DateTimeInterval(request.AvailabilityQueryParamaters.Start, request.AvailabilityQueryParamaters.End);

                for (int i = availableArticles.Count - 1; i >= 0; i--)
                {
                    if (availableArticles[i].AvailabilityDate != null && availableArticles[i].StartGap <= availableArticles[i].AvailabilityDate)
                    {
#pragma warning disable CS8629 // Nullable value type may be null.
                        availableArticles[i].StartGap = availableArticles[i].AvailabilityDate.Value;
#pragma warning restore CS8629 // Nullable value type may be null.
                    }

                    var interval2 = new DateTimeInterval(availableArticles[i].StartGap, availableArticles[i].EndGap);
                    var minutesIntersection = DateTimeUtils.GetIntervalIntersection(interval1, interval2);
                    if (minutesIntersection < minutesInterval)
                    {
                        availableArticles.RemoveAt(i);
                    }
                }
            }

            var availableArticleIds = availableArticles.Select(x => x.ArticleId).Distinct().ToList();
            return MergeOverlappingIntervals(availableArticles, availableArticleIds, request);
        }

        private IEnumerable<AvailabilityModel> MergeOverlappingIntervals(IEnumerable<Entities.EventGap> availableArticles, IEnumerable<Guid> availableArticleIds, ArticlesAvailabilityRequest request)
        {
            var availabilities = new List<AvailabilityModel>();
            foreach (var id in availableArticleIds)
            {
                var gaps = availableArticles.Where(x => x.ArticleId == id).Select(x =>
                new DateTimeInterval(
                    x.StartGap <= request.AvailabilityQueryParamaters.Start
                    ? CompareAvailabilityAndStartDate(x.AvailabilityDate, request.AvailabilityQueryParamaters.Start)
                    : CompareAvailabilityAndStartDate(x.AvailabilityDate, x.StartGap),
                    x.EndGap >= request.AvailabilityQueryParamaters.End ? request.AvailabilityQueryParamaters.End : x.EndGap)).OrderBy(x => x.From);
                var availability = new AvailabilityModel
                {
                    Id = id,
                    Gaps = DateTimeUtils.MergeOverlappingIntervals(gaps)
                    .Select(x => new GapModel { StartGap = x.From, EndGap = x.To })
                };
                availabilities.Add(availability);
            }

            return availabilities;
        }

        private DateTime CompareAvailabilityAndStartDate(DateTime? availabilityDate, DateTime? startDate)
        {
            if (availabilityDate == null || startDate == null)
            {
                throw new ArgumentException("ArticlesAvailabilityQuery.CompareAvailabilityAndStartDate - argument can not be null");
            }

            return availabilityDate.Value >= startDate.Value ? availabilityDate.Value : startDate.Value;
        }
    }
}
