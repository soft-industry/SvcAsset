using MediatR;
using Microsoft.EntityFrameworkCore;
using SvcAsset.Core.Interfaces;
using SvcAsset.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SvcAsset.Core.Queries.Reservation
{
    public class ReservationQuery : IRequestHandler<ReservationRequest, ReservationModel?>
    {
        private readonly Interfaces.IEFContext _ctx;

        public ReservationQuery(IEFContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ReservationModel?> Handle(ReservationRequest request, CancellationToken cancellationToken)
        {
            var evt = await _ctx.Events.SingleOrDefaultAsync(x => x.Id == request.EventId, cancellationToken);

            if (evt == null)
            {
                return null;
            }

            var maxEndDate = new DateTime(9999, 12, 31, 23, 45, 0);

            return new ReservationModel
            {
                Id = evt.Id,
                AssetId = evt.AssetId,
                ArticleId = evt.ArticleId,
                TenantId = evt.TenantId,
                EventTime = evt.EventTime != null ?
                new EventTimeModel
                {
                    TimeType = evt.EventTime.TimeType,
                    Start = evt.EventTime.Start,
                    End = evt.EventTime.End,
                    DurationType = evt.EventTime.DurationType,
                    Duration = evt.EventTime.Duration
                }
                : null,
                EventStart = evt.EventStart,
                EventEnd = evt.EventEnd == maxEndDate ? (DateTime?)null : evt.EventEnd,
                Purpose = evt.PurposeId,
                IsConfirmed = evt.IsConfirmed,
                HasUserAssetAssignment = evt.HasUserAssetAssignment,
                Comment = evt.Comment,
                EventLocation = evt.EventLocation != null ?
                new EventLocationModel
                {
                    Longitude = evt.EventLocation.X,
                    Latitude = evt.EventLocation.Y
                }
                : null,
                EventAddress = evt.EventAddress != null ?
                new AddressModel
                {
                    City = evt.EventAddress.City,
                    CountryName = evt.EventAddress.CountryName,
                    Street = evt.EventAddress.Street,
                    Zip = evt.EventAddress.ZIP
                }
                : new AddressModel(),
                EventLocationComment = evt.LocationComment,
                Customer = evt.Customer != null ?
                new CustomerModel(new AddressModel(), new CustomerContactModel())
                {
                    Comment = evt.Customer.Comment,
                    Name = evt.Customer.Name,
                    Number = evt.Customer.Number,
                    Address = evt.Customer.Address != null ?
                          new AddressModel
                          {
                              City = evt.Customer.Address.City,
                              CountryName = evt.Customer.Address.CountryName,
                              Street = evt.Customer.Address.Street,
                              Zip = evt.Customer.Address.ZIP
                          }
                          : new AddressModel(),
                    CustomerContact = evt.Customer.Contact != null ?
                     new CustomerContactModel
                     {
                         Name = evt.Customer.Contact.Name,
                         Telephone = evt.Customer.Contact.Tel
                     }
                     : new CustomerContactModel()
                }
                : new CustomerModel(new AddressModel(), new CustomerContactModel())
            };
        }
    }
}
