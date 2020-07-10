using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SvcAsset.Core.Commands.Event;
using SvcAsset.Core.Entities;
using SvcAsset.Core.Entities.Common;
using SvcAsset.Core.Models;
using System;
using System.Threading;

namespace UnitTests
{
    public class Tests
    {
        private CoreContext _ctx;
        private Guid _eventId = Guid.NewGuid();
        private Guid _assetId = Guid.NewGuid();
        private Guid _articleId = Guid.NewGuid();
        private Guid _tenantId = Guid.NewGuid();
        private Purpose _purposeId = Purpose.demo;
        private bool _hasUserAssetAssignment = true;
        private string _locationComment = "LocComment";
        private bool _isConfirmed = false;
        private string _comment = "Comment";
        private DurationType _durationType = DurationType.Months;
        private int _duration = 17;
        private string _conflictMsg = "Please use a different time-range. This one is already in use";
        private DateTime _startCommon = new DateTime(2020, 05, 10);
        private DateTime _endCommon = new DateTime(2020, 05, 15);

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<CoreContext>();
            builder.UseInMemoryDatabase("Core");
            var options = builder.Options;
            _ctx = new CoreContext(options);
        }

        [TearDown]
        public void Dispose()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
            }
        }

        [Test]
        public void CreateEventWithoutRequest_ThrowsException()
        {
            var cmd = new CreateEventCommand(_ctx);

            Assert.Throws<ArgumentNullException>(() => cmd.Handle(null, CancellationToken.None).Wait());
        }

        [Test]
        public void CreateEventWithoutModel_ThrowsException()
        {
            var cmd = new CreateEventCommand(_ctx);
            var request = new CreateEventRequest(null);

            Assert.Throws<ArgumentNullException>(() => cmd.Handle(request, CancellationToken.None).Wait());
        }

        [Test]
        public void CreateEventWithEmptyModel_ThrowsException()
        {
            var cmd = new CreateEventCommand(_ctx);
            var request = new CreateEventRequest(new ReservationModel());

            Assert.Throws<ArgumentNullException>(() => cmd.Handle(request, CancellationToken.None).Wait());
        }

        [Test]
        public void CreateEventWithoutEventTime_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => CreateEvent(null, _assetId));
        }

        [Test]
        public void CreateActiveEventWithCorrectDatesAndModel_Success()
        {
            var timeType = TimeType.Date;
            var start = new DateTime(2020, 07, 01);
            var end = new DateTime(2020, 07, 05);

            var eventTimeModel = CreateEventTimeModel(timeType, start: start, end: end);
            var response = CreateEvent(eventTimeModel, _assetId);

            AssertsForEachCorrectEvent(response, _assetId);
            AssertsForSpecificTimeType(response, timeType, start: start, end: end);
            Assert.AreEqual(EventStatus.Active, response.EventCreated.EventStatus);
        }

        [Test]
        public void CreateShortageEventWithCorrectDatesAndModel_Success()
        {
            var timeType = TimeType.Date;

            var eventTimeModel = CreateEventTimeModel(timeType, start: _startCommon, end: _endCommon);
            var response = CreateEvent(eventTimeModel);

            AssertsForEachCorrectEvent(response, null);
            AssertsForSpecificTimeType(response, timeType, start: _startCommon, end: _endCommon);
            Assert.AreEqual(EventStatus.Shortage, response.EventCreated.EventStatus);
        }

        [Test]
        public void CreatePendingEventWithCorrectDurationAndModel_Success()
        {
            var timeType = TimeType.Duration;

            var eventTimeModel = CreateEventTimeModel(timeType, durationType: _durationType, duration: _duration);
            var response = CreateEvent(eventTimeModel);

            AssertsForEachCorrectEvent(response, null);
            AssertsForSpecificTimeType(response, timeType, durationType: _durationType, duration: _duration);
            Assert.AreEqual(EventStatus.Pending, response.EventCreated.EventStatus);
        }

        [Test]
        public void CreateEventWithCorrectDurationAndModel_Success()
        {
            var timeType = TimeType.Duration;

            var eventTimeModel = CreateEventTimeModel(timeType, durationType: _durationType, duration: _duration);
            var response = CreateEvent(eventTimeModel, _assetId);

            AssertsForEachCorrectEvent(response, _assetId);
            AssertsForSpecificTimeType(response, timeType, durationType: _durationType, duration: _duration);
        }

        [Test]
        public void Create2EventsWithSameDates_ThrowsException()
        {
            var timeType = TimeType.Date;
            var start = new DateTime(2020, 08, 01);
            var end = new DateTime(2020, 08, 05);

            var eventTimeModel1 = CreateEventTimeModel(timeType, start: start, end: end);
            var response1 = CreateEvent(eventTimeModel1, _assetId);
            var eventTimeModel2 = CreateEventTimeModel(timeType, start: start, end: end);
            var response2 = CreateEvent(eventTimeModel2, _assetId);

            AssertsForEachCorrectEvent(response1, _assetId);
            AssertsForSpecificTimeType(response1, timeType, start: start, end: end);
            Assert.AreEqual(EventStatus.Active, response1.EventCreated.EventStatus);
            Assert.IsNotNull(response2);
            Assert.IsFalse(response2.Success);
            Assert.IsNull(response2.EventCreated);
            Assert.AreEqual(_conflictMsg, response2.ErrorMessage);
        }

        [Test]
        public void Create2EventsWithOverlappingDates_ThrowsException()
        {
            var timeType = TimeType.Date;
            var start1 = new DateTime(2020, 07, 10);
            var end1 = new DateTime(2020, 07, 15);
            var start2 = new DateTime(2020, 07, 07);
            var end2 = new DateTime(2020, 07, 12);

            var eventTimeModel1 = CreateEventTimeModel(timeType, start: start1, end: end1);
            var response1 = CreateEvent(eventTimeModel1, _assetId);
            var eventTimeModel2 = CreateEventTimeModel(timeType, start: start2, end: end2);
            var response2 = CreateEvent(eventTimeModel2, _assetId);

            AssertsForEachCorrectEvent(response1, _assetId);
            AssertsForSpecificTimeType(response1, timeType, start: start1, end: end1);
            Assert.AreEqual(EventStatus.Active, response1.EventCreated.EventStatus);
            Assert.IsNotNull(response2);
            Assert.IsFalse(response2.Success);
            Assert.IsNull(response2.EventCreated);
            Assert.AreEqual(_conflictMsg, response2.ErrorMessage);
        }

        [Test]
        public void Create2EventsWithOverlappingDates2_ThrowsException()
        {
            var timeType = TimeType.Date;
            var start1 = new DateTime(2020, 08, 10);
            var end1 = new DateTime(2020, 08, 15);
            var start2 = new DateTime(2020, 08, 11);
            var end2 = new DateTime(2020, 08, 13);

            var eventTimeModel1 = CreateEventTimeModel(timeType, start: start1, end: end1);
            var response1 = CreateEvent(eventTimeModel1, _assetId);
            var eventTimeModel2 = CreateEventTimeModel(timeType, start: start2, end: end2);
            var response2 = CreateEvent(eventTimeModel2, _assetId);

            AssertsForEachCorrectEvent(response1, _assetId);
            AssertsForSpecificTimeType(response1, timeType, start: start1, end: end1);
            Assert.AreEqual(EventStatus.Active, response1.EventCreated.EventStatus);
            Assert.IsNotNull(response2);
            Assert.IsFalse(response2.Success);
            Assert.IsNull(response2.EventCreated);
            Assert.AreEqual(_conflictMsg, response2.ErrorMessage);
        }

        [Test]
        public void Create3EventsWithOverlappingDates_ThrowsException()
        {
            var timeType = TimeType.Date;
            var start1 = new DateTime(2020, 09, 10);
            var end1 = new DateTime(2020, 09, 15);
            var start2 = new DateTime(2020, 09, 05);
            var end2 = new DateTime(2020, 09, 10);
            var start3 = new DateTime(2020, 09, 15);
            var end3 = new DateTime(2020, 09, 20);

            var eventTimeModel1 = CreateEventTimeModel(timeType, start: start1, end: end1);
            var response1 = CreateEvent(eventTimeModel1, _assetId);
            var eventTimeModel2 = CreateEventTimeModel(timeType, start: start2, end: end2);
            var response2 = CreateEvent(eventTimeModel2, _assetId);
            var eventTimeModel3 = CreateEventTimeModel(timeType, start: start3, end: end3);
            var response3 = CreateEvent(eventTimeModel3, _assetId);

            AssertsForEachCorrectEvent(response1, _assetId);
            AssertsForSpecificTimeType(response1, timeType, start: start1, end: end1);
            Assert.AreEqual(EventStatus.Active, response1.EventCreated.EventStatus);
            Assert.IsNotNull(response2);
            Assert.IsFalse(response2.Success);
            Assert.IsNull(response2.EventCreated);
            Assert.AreEqual(_conflictMsg, response2.ErrorMessage);
            Assert.IsNotNull(response3);
            Assert.IsFalse(response3.Success);
            Assert.IsNull(response3.EventCreated);
            Assert.AreEqual(_conflictMsg, response3.ErrorMessage);
        }

        [Test]
        public void Create2EventsWithCorrectDates_Success()
        {
            var timeType = TimeType.Date;
            var start1 = new DateTime(2020, 06, 10);
            var end1 = new DateTime(2020, 06, 15);
            var start2 = new DateTime(2020, 06, 16);
            var end2 = new DateTime(2020, 06, 20);

            var eventTimeModel1 = CreateEventTimeModel(timeType, start: start1, end: end1);
            var response1 = CreateEvent(eventTimeModel1, _assetId);
            var eventTimeModel2 = CreateEventTimeModel(timeType, start: start2, end: end2);
            var response2 = CreateEvent(eventTimeModel2, _assetId);

            AssertsForEachCorrectEvent(response1, _assetId);
            AssertsForSpecificTimeType(response1, timeType, start: start1, end: end1);
            Assert.AreEqual(EventStatus.Active, response1.EventCreated.EventStatus);
            AssertsForEachCorrectEvent(response2, _assetId);
            AssertsForSpecificTimeType(response2, timeType, start: start2, end: end2);
            Assert.AreEqual(EventStatus.Active, response2.EventCreated.EventStatus);
        }

        [Test]
        public void Create2DifferentEventsWithCorrectData_Success()
        {
            var timeType1 = TimeType.Date;
            var start1 = new DateTime(2020, 06, 01);
            var end1 = new DateTime(2020, 06, 07);
            var timeType2 = TimeType.Duration;

            var eventTimeModel1 = CreateEventTimeModel(timeType1, start: start1, end: end1);
            var response1 = CreateEvent(eventTimeModel1, _assetId);
            var eventTimeModel2 = CreateEventTimeModel(timeType2, durationType: _durationType, duration: _duration);
            var response2 = CreateEvent(eventTimeModel2, _assetId);

            AssertsForEachCorrectEvent(response1, _assetId);
            AssertsForSpecificTimeType(response1, timeType1, start: start1, end: end1);
            Assert.AreEqual(EventStatus.Active, response1.EventCreated.EventStatus);
            AssertsForEachCorrectEvent(response2, _assetId);
            AssertsForSpecificTimeType(response2, timeType2, durationType: _durationType, duration: _duration);
        }

        private void AssertsForEachCorrectEvent(CreateEventResponse response, Guid? assetId)
        {
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.IsNotNull(response.EventCreated);
            Assert.AreEqual(_eventId, response.EventCreated.EventId);
            Assert.AreEqual(assetId, response.EventCreated.AssetId);
            Assert.AreEqual(_articleId, response.EventCreated.ArticleId);
            Assert.AreEqual(_tenantId, response.EventCreated.TenantId);
            Assert.AreEqual(_purposeId, response.EventCreated.PurposeId);
            Assert.AreEqual(_hasUserAssetAssignment, response.EventCreated.HasUserAssetAssignment);
            Assert.AreEqual(_locationComment, response.EventCreated.LocationComment);
            Assert.AreEqual(_isConfirmed, response.EventCreated.IsConfirmed);
            Assert.AreEqual(_comment, response.EventCreated.Comment);
        }

        private void AssertsForSpecificTimeType(CreateEventResponse response, TimeType timeType, DurationType? durationType = null, DateTime? start = null, DateTime? end = null, int? duration = null)
        {
            Assert.IsNotNull(response.EventCreated.EventTime);
            Assert.AreEqual(timeType, response.EventCreated.EventTime.TimeType);

            switch (timeType)
            {
                case TimeType.Date: 
                    {
                        Assert.AreEqual(start, response.EventCreated.EventTime.Start);
                        Assert.AreEqual(end, response.EventCreated.EventTime.End);
                        Assert.IsNull(response.EventCreated.EventTime.DurationType);
                        Assert.IsNull(response.EventCreated.EventTime.Duration);

                        break;
                    }

                case TimeType.Duration: 
                    {
                        Assert.AreEqual(durationType, response.EventCreated.EventTime.DurationType);
                        Assert.AreEqual(duration, response.EventCreated.EventTime.Duration);
                        Assert.IsNull(response.EventCreated.EventTime.Start);
                        Assert.IsNull(response.EventCreated.EventTime.End);

                        break; 
                    }
            }
        }

        private EventTimeModel CreateEventTimeModel(TimeType timeType, DurationType? durationType = null, DateTime? start = null, DateTime? end = null, int? duration = null)
        {
            return new EventTimeModel
            {
                TimeType = timeType,
                Start = start,
                End = end,
                DurationType = durationType,
                Duration = duration                
            };
        }

        private CreateEventResponse CreateEvent(EventTimeModel eventTimeModel, Guid? assetId = null)
        {
            CreateEventCommand cmd = new CreateEventCommand(_ctx);

            var model = new ReservationModel
            {
                Id = _eventId,
                ArticleId = _articleId,
                AssetId = assetId,
                Comment = _comment,
                HasUserAssetAssignment = _hasUserAssetAssignment,
                EventLocationComment = _locationComment,
                IsConfirmed = _isConfirmed,
                Purpose = _purposeId,
                TenantId = _tenantId,
                EventTime = eventTimeModel
            };
            
            var request = new CreateEventRequest(model);

            var res = cmd.Handle(request, CancellationToken.None).Result;

            return res;
        }
    }
}