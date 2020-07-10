using SvcAsset.Core.Entities.Common;

namespace SvcAsset.Core.Entities
{
    public class Address : BaseEntity // to fix "The entity type requires a primary key to be defined"
    {
        public Address(string? street, string? zip, string? city, string? countryName)
        {
            Street = street;
            ZIP = zip;
            City = city;
            CountryName = countryName;
        }

        public string? Street { get; set; }

        public string? ZIP { get; set; }

        public string? City { get; set; }

        public string? CountryName { get; set; }

        private Address() { }
    }
}
