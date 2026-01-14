using GiftRide.Infrastructure.Data.Entities;

namespace GiftRide.Core.Contracts
{
    public interface IReservationService
    {
        Voucher GetVoucherForReservation(int voucherId, string userId);
        bool MakeReservation(int voucherId, DateTime date, string userId);

    }
}
