using System.Collections.Generic;

namespace SvcAsset.Core.Models
{
    public class AvailabilitiesModel
    {
        public IEnumerable<AvailabilityModel> Availabilities { get; set; } = new List<AvailabilityModel>();
    }
}
