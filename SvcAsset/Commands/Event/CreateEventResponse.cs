namespace SvcAsset.Core.Commands.Event
{
    public class CreateEventResponse
    {
        public bool Success { get; internal set; }

        public Entities.Event? EventCreated { get; internal set; }

        public string ErrorMessage { get; internal set; }
    }
}
