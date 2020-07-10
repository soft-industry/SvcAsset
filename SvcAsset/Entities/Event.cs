using Ardalis.GuardClauses;
using NetTopologySuite.Geometries;
using SvcAsset.Core.Entities.Common;
using SvcAsset.Core.Helpers;
using SvcAsset.Core.Models;
using System;

namespace SvcAsset.Core.Entities
{
    public class Event : BaseEntity, ITenantEntity, ISoftDeletionEntity
    {
        public Guid? AssetId { get; private set; }

        public Guid? ArticleId { get; private set; }

        public Guid? EventId { get; private set; }

        public Guid? TenantId { get; private set; }

        public EventTime EventTime { get; private set; }

        public Purpose PurposeId { get; private set; }

        public Point? EventLocation { get; private set; }

        public Address? EventAddress { get; private set; }

        public string? LocationComment { get; private set; }

        public Customer? Customer { get; private set; }

        public string? Comment { get; private set; }

        public bool HasUserAssetAssignment { get; private set; }

        public EventStatus EventStatus { get; private set; }

        public bool IsConfirmed { get; private set; }

        public EventLock? EventLock { get; private set; }

        public DateTime LU_Date { get; private set; }

        public Guid LU_User { get; private set; }

        public bool IsDeleted { get; private set; }

        #region Old
        public DateTime EventStart { get; private set; }

        public DateTime EventEnd { get; private set; }
        #endregion

        internal Event(ReservationModel reservation)
        {
            UpdateModel(reservation);
        }

        public void Update(ReservationModel reservation)
        {
            UpdateModel(reservation);
        }

        public void SoftDelete()
        {
            IsDeleted = true;
        }

        public void SoftRestore()
        {
            IsDeleted = false;
        }

        private void UpdateModel(ReservationModel reservation)
        {
            Guard.Against.Null(reservation, nameof(reservation));
            Guard.Against.GuidNullOrEmpty(reservation.Id, nameof(reservation.Id));
            Guard.Against.Null(reservation.EventTime, nameof(reservation.EventTime));

            EventId = reservation.Id;
            AssetId = reservation.AssetId;
            ArticleId = reservation.ArticleId;
            TenantId = reservation.TenantId;
            PurposeId = reservation.Purpose;
            HasUserAssetAssignment = reservation.HasUserAssetAssignment;
            LocationComment = reservation.EventLocationComment;
            IsConfirmed = reservation.IsConfirmed;
            Comment = reservation.Comment;

            EventTime = new EventTime(reservation.EventTime.TimeType, reservation.EventTime.DurationType, reservation.EventTime.Start, reservation.EventTime.End, reservation.EventTime.Duration);

            if (EventTime.TimeType == TimeType.Date && !AssetId.HasValue && EventId.HasValue)
            {
                EventStatus = EventStatus.Shortage;
            }
            else if (EventTime.TimeType == TimeType.Date && AssetId.HasValue && EventId.HasValue)
            {
                EventStatus = EventStatus.Active;
            }
            else if (EventTime.TimeType == TimeType.Duration && !AssetId.HasValue && EventId.HasValue)
            {
                EventStatus = EventStatus.Pending;
            }

            if (reservation.EventAddress != null)
            {
                EventAddress = new Address(reservation.EventAddress.Street, reservation.EventAddress.Zip, reservation.EventAddress.City, reservation.EventAddress.CountryName);
            }

            if (reservation.EventLocation != null)
            {
                EventLocation = new Point(reservation.EventLocation.Longitude, reservation.EventLocation.Latitude);
                EventLocation.SRID = 4326;
            }

            if (reservation.Customer != null)
            {
                var newCustomer = new Customer(reservation.Customer.Number, reservation.Customer.Name, reservation.Customer.Comment);
                if (reservation.Customer.CustomerContact != null)
                {
                    var newContact = new CustomerContact(reservation.Customer.CustomerContact.Name, reservation.Customer.CustomerContact.Telephone);
                    newCustomer.SetContact(newContact);
                }

                if (reservation.Customer.Address != null)
                {
                    var newAddress = new Address(reservation.Customer.Address.Street, reservation.Customer.Address.Zip, reservation.Customer.Address.City, reservation.Customer.Address.CountryName);
                    newCustomer.SetAddress(newAddress);
                }

                Customer = newCustomer;
            }

            // TODO: something here for EventLock & Storing user id
        }

        private Event() { }
    }
}
