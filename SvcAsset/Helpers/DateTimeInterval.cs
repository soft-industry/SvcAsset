﻿using System;

namespace SvcAsset.Core.Helpers
{
    public class DateTimeInterval
    {
        public DateTimeInterval(DateTime? from, DateTime? to)
        {
            From = from;
            To = to;
        }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }
}
