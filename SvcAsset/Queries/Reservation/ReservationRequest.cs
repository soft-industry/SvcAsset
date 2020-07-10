using MediatR;
using SvcAsset.Core.Models;
using System;

namespace SvcAsset.Core.Queries.Reservation
{
    public class ReservationRequest : IRequest<ReservationModel?>
    {
        public Guid EventId { get; set; }

        public ReservationRequest(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
