using Newtonsoft.Json.Converters;
using SvcAsset.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SvcAsset.Core.Queries.Parameters
{
    public class AvailabilityQueryParamaters
    {
        [Required]
        public DateTime? Start { get; set; }

        [Required]
        public DateTime? End { get; set; }

        public int Number { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Unit Unit { get; set; }
    }
}
