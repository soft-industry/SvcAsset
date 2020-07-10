using SvcAsset.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SvcAsset.Core.Helpers
{
    public static class DateTimeUtils
    {
        public static double GetIntervalIntersection(DateTimeInterval interval1, DateTimeInterval interval2)
        {
            if (interval1.From == null || interval1.To == null || interval2.From == null || interval2.To == null)
            {
                throw new ArgumentException("DateTimeUtils.GetIntervalIntersection - argument can not be null");
            }

            DateTime from = interval1.From.Value < interval2.From.Value
                   ? interval2.From.Value
                   : interval1.From.Value;

            DateTime to = interval1.To.Value > interval2.To.Value
                ? interval2.To.Value
                : interval1.To.Value;

            return (to - from).TotalMinutes;
        }

        public static double GetIntervalForUnit(Unit unit, int number)
        {
            double minutesInterval = 0;
            switch (unit)
            {
                case Entities.Unit.d:
                    minutesInterval = number * 24 * 60;
                    break;
                case Entities.Unit.w:
                    minutesInterval = number * 7 * 24 * 60;
                    break;
                case Entities.Unit.m:
                    minutesInterval = number * 30 * 24 * 60;
                    break;
                case Entities.Unit.y:
                    minutesInterval = number * 365 * 24 * 60;
                    break;
            }

            return minutesInterval;
        }

        public static IEnumerable<DateTimeInterval> MergeOverlappingIntervals(IEnumerable<DateTimeInterval> intervals)
        {
            var accumulator = intervals.First();
            intervals = intervals.Skip(1);

            foreach (var interval in intervals)
            {
                if (interval.From <= accumulator.To)
                {
                    accumulator = Combine(accumulator, interval);
                }
                else
                {
                    yield return accumulator;
                    accumulator = interval;
                }
            }

            yield return accumulator;
        }

        private static DateTimeInterval Combine(DateTimeInterval start, DateTimeInterval end)
        {
            return new DateTimeInterval(start.From, Max(start.To, end.To));
        }

        private static DateTime Max(DateTime? left, DateTime? right)
        {
            if (left == null || right == null)
            {
                throw new ArgumentException("DateTimeUtils.Max - argument can not be null");
            }

            return (left.Value > right.Value) ? left.Value : right.Value;
        }
    }
}
