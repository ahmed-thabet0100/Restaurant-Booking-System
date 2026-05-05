using Booking.Data.Entities;
using Booking.Infra.Specifications.ReservationSpecification;
using Booking.Infra.Specifications.ReservationSpecification.User;
using Restaurant.Service;

namespace Booking.Service.Abstracts
{
    public interface IBookService
    {
        Task<Response<Reservation>> GetReservation(int reservationId);
        Task<Response<(IReadOnlyList<Reservation>, int Count)>> GetAllReservationForUser(ReservationSpecParamsUser specParams);
        Task<Response<(IReadOnlyList<Reservation>, int Count)>> GetAllReservationForStaff(ReservationSpecParamsStaff specParams);
        Task<Response<Reservation>> CreateReservation(Reservation reservation);
        Task<Response<Reservation>> UpdateReservationTime(Reservation reservation);
        Task<Response<string>> CancelReservation(int ReservationId);
        // staff
        Task<Response<Reservation>> ApproveReservation(int reservationId, int tableId);
        Task<Response<string>> RejectReservation(int reservationId);
        Task<Response<string>> CompleteReservation(int reservationId);
        Task AutoCancel();
        Task AutoCompelete();
        Task RememberUser();

    }
}
