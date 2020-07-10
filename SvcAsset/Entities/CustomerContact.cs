using SvcAsset.Core.Entities.Common;

namespace SvcAsset.Core.Entities
{
    public class CustomerContact : BaseEntity // to fix "The entity type requires a primary key to be defined"
    {
        public CustomerContact(string? name, string? tel)
        {
            Name = name;
            Tel = tel;
        }

        public string? Name { get; private set; }

        public string? Tel { get; private set; }
    }
}
