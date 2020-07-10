using NUnit.Framework;
using SvcAsset.Core.Entities;
using System;

namespace UnitTests
{
    [TestFixture]
    public class EventTimeTests
    {
        private DateTime _start = new DateTime(2020, 07, 10);
        private DateTime _end = new DateTime(2020, 07, 15);
        private DurationType _durationType = DurationType.Weeks;
        private int _duration = 1;

        [Test]
        public void CreateEventTimeWhenPassedEmptyData_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new EventTime(TimeType.Date));
        }

        [Test]
        public void CreateEventTimeWhenPassedWrongData_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new EventTime(TimeType.Duration, start: _start, end: _end));
        }

        [Test]
        public void CreateEventTimesWhenPassedWrongDuration_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new EventTime(TimeType.Duration, durationType: _durationType, duration: -100));
            Assert.Throws<ArgumentOutOfRangeException>(() => new EventTime(TimeType.Duration, durationType: _durationType, duration: 0));
        }

        [Test]
        public void CreateEventTimeWhenPassedCorrectDates_Success()
        {
            var timeType = TimeType.Date;
            
            var eventTime = new EventTime(timeType, start: _start, end: _end);

            Assert.IsNotNull(eventTime);
            Assert.AreEqual(timeType, eventTime.TimeType);
            Assert.AreEqual(_start, eventTime.Start.Value);
            Assert.AreEqual(_end, eventTime.End.Value);
            Assert.IsNull(eventTime.DurationType);
            Assert.IsNull(eventTime.Duration);
        }

        [Test]
        public void CreateEventTimeWhenPassedCorrectDuration_Success()
        {
            var timeType = TimeType.Duration;

            var eventTime = new EventTime(timeType, durationType: _durationType, duration: _duration);

            Assert.IsNotNull(eventTime);
            Assert.AreEqual(timeType, eventTime.TimeType);
            Assert.AreEqual(_durationType, eventTime.DurationType.Value);
            Assert.AreEqual(_duration, eventTime.Duration.Value);
            Assert.IsNull(eventTime.Start);
            Assert.IsNull(eventTime.End);
        }
    }
}
