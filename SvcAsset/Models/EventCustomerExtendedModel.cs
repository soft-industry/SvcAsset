namespace SvcAsset.Core.Models
{
    public class EventCustomerExtendedModel : EventCustomerModel
    {
        public string? Number { get; set; }

        public AddressModel? Address { get; set; }
    }
}
