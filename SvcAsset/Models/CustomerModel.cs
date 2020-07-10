namespace SvcAsset.Core.Models
{
    public class CustomerModel
    {
        public CustomerModel(AddressModel address, CustomerContactModel customerContact)
        {
            Address = address;
            CustomerContact = customerContact;
        }

        public string? Number { get; set; }

        public string Name { get; set; } = string.Empty;

        public AddressModel? Address { get; set; }

        public CustomerContactModel? CustomerContact { get; set; }

        public string? Comment { get; set; }
    }
}
