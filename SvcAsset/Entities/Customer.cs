using SvcAsset.Core.Entities.Common;

namespace SvcAsset.Core.Entities
{
    public class Customer : BaseEntity // to fix "The entity type requires a primary key to be defined"
    {
        public Customer(string? number, string name, string? comment)
        {
            Number = number;
            Name = name;
            Comment = comment;
        }

        public string? Number { get; private set; }

        public string Name { get; private set; }

        public string? Comment { get; private set; }

        public Address? Address { get; private set; }

        public CustomerContact? Contact { get; private set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private Customer() { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public void SetContact(CustomerContact contact)
        {
            Contact = contact;
        }

        public void SetAddress(Address address)
        {
            Address = address;
        }
    }
}
