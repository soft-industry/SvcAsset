using MediatR;
using SvcAsset.Core.Models;

namespace SvcAsset.Core.Commands.Event
{
    public class CreateEventRequest : IRequest<CreateEventResponse>
    {
        public ReservationModel CreateEvent { get; set; }

        public CreateEventRequest(ReservationModel createEvent)
        {
            CreateEvent = createEvent;
        }
    }
}
