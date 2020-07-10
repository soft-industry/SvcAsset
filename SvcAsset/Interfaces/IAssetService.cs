using SvcAsset.Core.Models;
using System;

namespace SvcAsset.Core.Interfaces
{
    public interface IAssetService
    {
        Guid SelectAssetByAvailabilityCriteria(ReservationModel reservationModel, Guid tenantId, Guid assetId = default);
    }
}
