using MediatR;
using SvcAsset.Core.Entities;
using SvcAsset.Core.Models;
using SvcAsset.Core.Queries.Common;
using SvcAsset.Core.Queries.Parameters;
using System;

namespace SvcAsset.Core.Queries.Reservations
{
    public class ReservationsRequest : PagedRequest, IRequest<PagedResponse<Event, SearchReservationModel>>
    {
        public ReservationQueryParameters? ReservationQueryParameters { get; }

        public string? AssetId { get; }

        public Guid TenantId { get; }

        public ReservationsRequest(PaginationParameters paginationParameters)
        {
            PaginationParameters = paginationParameters;
        }

        public ReservationsRequest(
            PaginationParameters paginationParameters,
            ReservationQueryParameters reservationQueryParameters,
            Guid tenantId,
            string? assetId = null)
            : this(paginationParameters)
        {
            ReservationQueryParameters = reservationQueryParameters;
            TenantId = tenantId;
            AssetId = assetId;
        }
    }
}
