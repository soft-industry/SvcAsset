using Ardalis.GuardClauses;
using MediatR;
using SvcAsset.Core.Entities;
using SvcAsset.Core.Interfaces;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Commands.Event
{
    public class CreateEventCommand : IRequestHandler<CreateEventRequest, CreateEventResponse>
    {
        private readonly IEFContext _context;

        public CreateEventCommand(IEFContext context)
        {
            _context = context;
        }

        public Task<CreateEventResponse> Handle(CreateEventRequest request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));
            Guard.Against.Null(request.CreateEvent, nameof(request.CreateEvent));
            Guard.Against.Null(request.CreateEvent.EventTime, nameof(request.CreateEvent.EventTime));

            if (request.CreateEvent.EventTime.TimeType == TimeType.Date)
            {
                var newEventTime = request.CreateEvent.EventTime;

                // Get all events for the specified AssetId
                var events = _context.Events.Where(x => request.CreateEvent.AssetId.HasValue && x.AssetId == request.CreateEvent.AssetId);

                if (events != null && events.Count() > 0)
                {
                    // Select all existing EventTimes where TimeType is Date
                    var eventTimes = events.Select(x => x.EventTime).Where(x => x.TimeType == TimeType.Date);

                    if (eventTimes != null && eventTimes.Count() > 0)
                    {
                        foreach (var item in eventTimes)
                        {
                            if ((newEventTime.Start.Value >= item.Start.Value && newEventTime.Start.Value <= item.End.Value)
                                || (newEventTime.End.Value >= item.Start.Value && newEventTime.End.Value <= item.End.Value))
                            {
                                return Task.FromResult(new CreateEventResponse
                                {
                                    Success = false,
                                    EventCreated = null,
                                    ErrorMessage = "Please use a different time-range. This one is already in use"
                                });
                            }
                        }
                    }
                }
            }

            var newEvent = new Entities.Event(request.CreateEvent);

            return HandleIntern(newEvent, cancellationToken);
        }

        private async Task<CreateEventResponse> HandleIntern(Entities.Event evt, CancellationToken cancellationToken)
        {
            _context.Events.Add(evt);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateEventResponse
            {
                Success = true,
                EventCreated = evt
            };
        }
    }
}
