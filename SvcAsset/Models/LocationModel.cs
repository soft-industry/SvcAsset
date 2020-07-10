using System;

namespace SvcAsset.Core.Models
{
    public class LocationModel
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime LocatingTime { get; set; }

        public bool IsCreatedManually { get; set; }

        public string? ClientOSInfo { get; set; }

        public string? ClientBrowserInfo { get; set; }
    }
}
