using MediatR;
using SvcAsset.Core.Entities;
using SvcAsset.Core.Interfaces;
using SvcAsset.Core.Models;
using SvcAsset.Core.Queries.Common;
using SvcAsset.Core.Queries.Parameters;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Queries.Reservations
{
    public class ReservationsQuery : IRequestHandler<ReservationsRequest, PagedResponse<Event, SearchReservationModel>>
    {
        private readonly Interfaces.IEFContext _ctx;

        public bool IsCompact { get; private set; }

        private DateTime MaxDateTime
        {
            get
            {
                return new DateTime(9999, 12, 31, 23, 45, 0);
            }
        }

        public ReservationsQuery(IEFContext ctx)
        {
            _ctx = ctx;
            IsCompact = false;
        }

        public async Task<PagedResponse<Event, SearchReservationModel>> Handle(
            ReservationsRequest request,
            CancellationToken cancellationToken)
        {
            IQueryable<Event> query = _ctx.Events;

            if (request.ReservationQueryParameters != null)
            {
                query = ApplyFilters(query, request.ReservationQueryParameters, request.TenantId, request.AssetId);
                IsCompact = request.ReservationQueryParameters.Compact;
            }

            return await PagedResponse<Event, SearchReservationModel>.Create(
            MapFrom,
            query,
            request.PaginationParameters.PageNumber,
            request.PaginationParameters.PageSize,
            cancellationToken);
        }

        private IQueryable<Event> ApplyFilters(
          IQueryable<Event> query,
          ReservationQueryParameters reservationQueryParameters,
          Guid tenantId,
          string? assetId)
        {
            if (reservationQueryParameters.ArticleId != null)
            {
                var assets = _ctx.Assets.Where(x => x.ArticleId == reservationQueryParameters.ArticleId && x.HolderTenantId == tenantId).Select(x => x.Id);
                query = query.Where(x => x.AssetId.HasValue && assets.Contains(x.AssetId.Value));
            }

            if (reservationQueryParameters.Start != null && reservationQueryParameters.End == null)
            {
                query = query.Where(x => x.EventEnd >= reservationQueryParameters.Start);
            }

            if (reservationQueryParameters.End != null && reservationQueryParameters.Start == null)
            {
                query = query.Where(x => x.EventStart <= reservationQueryParameters.End).OrderByDescending(x => x.EventStart);
            }

            if (reservationQueryParameters.Start != null && reservationQueryParameters.End != null)
            {
                query = query.Where(x => x.EventEnd > reservationQueryParameters.Start && x.EventStart < reservationQueryParameters.End);
            }

            if (!string.IsNullOrEmpty(assetId))
            {
                var asset = GetAssetById(assetId);
                if (asset != null)
                {
                    query = query.Where(x => x.AssetId == asset.Id);
                }
            }

            return query;
        }

        private Entities.Asset? GetAssetById(string? asset)
        {
            IQueryable<Entities.Asset> query;

            if (Guid.TryParse(asset, out Guid assetId))
            {
                query = _ctx.Assets.Where(x => x.Id == assetId);
            }
            else
            {
                query = _ctx.Assets.Where(x => x.LMID == asset);
            }

            return query.FirstOrDefault();
        }

        private SearchReservationModel MapFrom(Event evt)
        {
            EventCustomerModel? customer = null;
            if (evt.Customer != null)
            {
                if (IsCompact)
                {
                    customer = new EventCustomerCompactModel
                    {
                        Name = evt.Customer.Name
                    };
                }
                else
                {
                    customer = new EventCustomerExtendedModel
                    {
                        Name = evt.Customer.Name,
                        Number = evt.Customer.Number,
                        Address = evt.Customer.Address != null
                         ? new AddressModel
                         {
                             City = evt.Customer.Address.City,
                             CountryName = evt.Customer.Address.CountryName,
                             Street = evt.Customer.Address.Street,
                             Zip = evt.Customer.Address.ZIP
                         }
                         : null
                    };
                }
            }

            return new SearchReservationModel
            {
                Id = evt.Id,
                AssetId = evt.AssetId ?? Guid.Empty,
                EventStart = evt.EventStart,
                EventEnd = evt.EventEnd == MaxDateTime ? (DateTime?)null : evt.EventEnd,
                Purpose = evt.PurposeId.ToString(),
                HasUserAssetAssignment = evt.HasUserAssetAssignment,
                Customer = customer,
                EventAddress = evt.EventAddress != null ? new EventAddressModel { City = evt.EventAddress.City } : null
            };
        }
    }
}