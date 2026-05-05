using Booking.Data.Entities;
using Booking.Infra.Specifications.ReservationSpecification;
using Booking.Infra.Specifications.ReservationSpecification.Staff;
using Booking.Infra.Specifications.ReservationSpecification.User;
using Booking.Service.Abstracts;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Dtos;
using Restaurant.Data.Entities.Identity;
using Restaurant.Infra.Abstracts;
using Restaurant.Service;
using Restaurant.Service.Abstracts;
using Talabat.Core.Repo.Contarct;

namespace Booking.Service.Implementation
{
    public class BookService : IBookService
    {
        private readonly ResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<AppUser> _userManager;
        private readonly IReservationRepo _ReservationRepo;
        private readonly INotificationService _notificationService;

        public BookService(ResponseHandler responseHandler, IUnitOfWork unitOfWork,
            ICurrentUserService currentUser, UserManager<AppUser> userManager,
            IReservationRepo reservationRepo,
            INotificationService notificationService)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _userManager = userManager;
            _ReservationRepo = reservationRepo;
            _notificationService = notificationService;
        }
        public async Task<Response<(IReadOnlyList<Reservation>, int Count)>> GetAllReservationForUser(ReservationSpecParamsUser specParams)
        {
            if (_currentUser.UserId == null)
                return _responseHandler.Unauthorized<(IReadOnlyList<Reservation>, int)>();
            var UserId = _currentUser.UserId;

            var reservations = await _unitOfWork.Repository<Reservation>()
                .GetAllEntitiesWithSpecAsync(new GetAllReservationForUserSpecification(specParams, UserId));

            var Count = await _unitOfWork.Repository<Reservation>()
                .CountAsync(new ReservationForCounfSpec(specParams, UserId));

            if (reservations == null || !reservations.Any())
                return _responseHandler.Success
                <(IReadOnlyList<Reservation>, int)>
                ((reservations, Count));


            return _responseHandler.Success
                <(IReadOnlyList<Reservation>, int)>
                ((reservations, Count));

        }
        public async Task<Response<(IReadOnlyList<Reservation>, int Count)>> GetAllReservationForStaff(ReservationSpecParamsStaff specParams)
        {
            if (_currentUser.RestaurantId == null)
                return _responseHandler.Unauthorized<(IReadOnlyList<Reservation>, int)>();
            var RestaurantId = _currentUser.RestaurantId;

            var reservations = await _unitOfWork.Repository<Reservation>()
                .GetAllEntitiesWithSpecAsync(new GetAllReservationForStaffSpecefication(specParams, RestaurantId.Value));

            if (reservations == null || !reservations.Any())
                return _responseHandler.NotFound<(IReadOnlyList<Reservation>, int)>("No Books Found");

            var Count = await _unitOfWork.Repository<Reservation>()
                .CountAsync(new ReservationForCounfSpecForStaff(specParams, RestaurantId.Value));

            return _responseHandler.Success
                <(IReadOnlyList<Reservation>, int)>
                ((reservations, Count));
        }

        public async Task<Response<Reservation>> GetReservation(int reservationId)
        {
            if (_currentUser.UserId == null)
                return _responseHandler.Unauthorized<Reservation>();

            var reservation = await _unitOfWork.Repository<Reservation>()
                .GetAsync(reservationId);

            if (reservation == null)
                return _responseHandler.NotFound<Reservation>("Reservation not found");

            // 🔐 check access
            if (_currentUser.Role == "User" && reservation.UserId != _currentUser.UserId)
                return _responseHandler.Unauthorized<Reservation>();

            if (_currentUser.Role == "Staff" && reservation.RestaurantId != _currentUser.RestaurantId)
                return _responseHandler.Unauthorized<Reservation>();

            return _responseHandler.Success(reservation);
        }
        //
        public async Task<Response<Reservation>> CreateReservation(Reservation reservation)
        {
            if (_currentUser.UserId == null)
                return _responseHandler.Unauthorized<Reservation>();

            reservation.UserId = _currentUser.UserId;

            var restaurant = await _unitOfWork.Repository<Book.Data.Entities.Restaurant>()
                .GetAsync(reservation.RestaurantId);

            if (restaurant == null)
                return _responseHandler.NotFound<Reservation>("Restaurant Not Found");

            if (reservation.NumberOfGuests <= 0)
                return _responseHandler.BadRequest<Reservation>("Invalid number of guests");

            if (reservation.StartDateTime < DateTime.UtcNow)
                return _responseHandler.BadRequest<Reservation>("Invalid reservation time");

            if (reservation.EndDateTime <= reservation.StartDateTime)
                return _responseHandler.BadRequest<Reservation>(
                    "Invalid reservation duration");

            if (reservation.StartDateTime <= DateTime.UtcNow.AddMinutes(30))
            {
                return _responseHandler.BadRequest<Reservation>(
                    "Cannot update reservation before 30 minutes");
            }
            if ((reservation.EndDateTime - reservation.StartDateTime).TotalHours > 1.5)
                return _responseHandler.BadRequest<Reservation>(
                    "Maximum number of hours for reservation Is 1.5");
            if (reservation.NumberOfGuests > 20)
                return _responseHandler.BadRequest<Reservation>("Maximum number of guests is 20");

            reservation.Status = ReservationStatus.Pending;
            reservation.CreatedAt = DateTime.UtcNow;
            reservation.UserName = _currentUser.UserName;
            reservation.BookNum = reservation.BookNum + 1;
            await _unitOfWork.Repository<Reservation>().AddAsync(reservation);


            await _notificationService.SendAsync(new NotificationDto
            {
                Type = NotificationTypes.NewReservation,
                CreatedAt = DateTime.UtcNow,
                Title = "New Reservation",
                Message = "A new reservation has been created"
            }, $"staff-{reservation.RestaurantId}");

            await _unitOfWork.CompleteAsync();
            return _responseHandler.Success(reservation, "Reservation created successfully");
        }
        public async Task<Response<Reservation>> UpdateReservationTime(Reservation Reservation)
        {
            if (_currentUser.UserId == null)
                return _responseHandler.Unauthorized<Reservation>();

            var reservation = await _unitOfWork.Repository<Reservation>()
                 .GetEntityWithSpecAsync(new GetReservationForUserSpecefication(Reservation.Id, _currentUser.UserId));

            if (reservation == null)
                return _responseHandler.NotFound<Reservation>("Reservation Not Found");

            // نتأكد إن الحجز بتاعه
            if (reservation.UserId != _currentUser.UserId)
                return _responseHandler.Unauthorized<Reservation>();

            // مينفعش يتعدل لو خلص أو اترفض
            if (reservation.Status == ReservationStatus.Completed ||
                reservation.Status == ReservationStatus.Rejected ||
                reservation.Status == ReservationStatus.Cancelled)
            {
                return _responseHandler.BadRequest<Reservation>(
                    "Reservation cannot be updated");
            }

            // الوقت الجديد
            if (reservation.StartDateTime < DateTime.UtcNow)
                return _responseHandler.BadRequest<Reservation>(
                    "Invalid reservation time");

            reservation.TableNumber = null;
            reservation.StartDateTime = reservation.StartDateTime;
            reservation.EndDateTime = reservation.EndDateTime;

            // يرجع pending عشان staff يراجع تاني
            reservation.Status = ReservationStatus.Pending;

            await _notificationService.SendAsync(new NotificationDto
            {
                Type = NotificationTypes.ReservationUpdated,
                Title = "Reservation Update",
                CreatedAt = DateTime.UtcNow,
                Message = "User Update Reservation Time"
            }, $"staff-{reservation.RestaurantId}");

            await _unitOfWork.CompleteAsync();
            return _responseHandler.Success(
                reservation,
                "Reservation updated successfully");
        }
        public async Task<Response<string>> CancelReservation(int reservationId)
        {
            if (_currentUser.UserId == null)
                return _responseHandler.Unauthorized<string>();

            var reservation = await _unitOfWork.Repository<Reservation>()
                .GetEntityWithSpecAsync(new GetReservationForUserSpecefication(reservationId, _currentUser.UserId));

            if (reservation == null)
                return _responseHandler.NotFound<string>(
                    "Reservation Not Found");

            if (reservation.UserId != _currentUser.UserId)
                return _responseHandler.Unauthorized<string>();

            if (reservation.Status == ReservationStatus.Completed)
                return _responseHandler.BadRequest<string>(
                    "Reservation already completed");

            if (reservation.StartDateTime <= DateTime.UtcNow.AddMinutes(20))
                return _responseHandler.BadRequest<string>("You Can`t Change Reservation Time");


            reservation.Status = ReservationStatus.Cancelled;
            // 🔥 تحديث الـ reservations للstaff
            await _notificationService.SendAsync(new NotificationDto
            {
                CreatedAt = DateTime.UtcNow,
                Title = NotificationTypes.ReservationUpdated,
                Type = NotificationTypes.ReservationCancelled,
                Message = "User Clanel Reservation"
            }, $"staff-{reservation.RestaurantId}");

            await _unitOfWork.CompleteAsync();


            return _responseHandler.Success(
                "Reservation cancelled successfully");
        }

        public async Task<Response<Reservation>> ApproveReservation(int reservationId, int tableNum)
        {
            if (_currentUser.RestaurantId == null)
                return _responseHandler.Unauthorized<Reservation>();

            var restaurantId = _currentUser.RestaurantId.Value;

            var reservation = await _unitOfWork.Repository<Reservation>()
                .GetEntityWithSpecAsync(
                    new GetReservationForStaffSpecefication(
                        reservationId,
                        restaurantId));

            if (reservation == null)
                return _responseHandler.NotFound<Reservation>(
                    "Reservation Not Found");

            if (reservation.Status != ReservationStatus.Pending)
                return _responseHandler.BadRequest<Reservation>(
                    "Already processed");

            // 🔥 check table availability
            var isAvailable = await _ReservationRepo.IsTableAvilable(
                reservation.RestaurantId,
                tableNum,
                reservation.StartDateTime,
                reservation.EndDateTime);

            if (!isAvailable)
            {
                return _responseHandler.BadRequest<Reservation>(
                    "Table is no longer available");
            }

            // assign table
            reservation.TableNumber = tableNum;
            reservation.AssignedAt = DateTime.UtcNow;
            reservation.AssignedBy = _currentUser.UserName;

            reservation.Status = ReservationStatus.Approved;
            reservation.ReminderSend = true;

            await _notificationService.SendAsync(new NotificationDto
            {
                CreatedAt = DateTime.UtcNow,
                Title = NotificationTypes.ReservationApproved,
                Type = NotificationTypes.ReservationApproved,
                Message = $"Your Reservation  {reservation.Id} has been Approved "
            }, $"user{reservation.UserId}");

            await _notificationService.SendAsync(new NotificationDto
            {
                CreatedAt = DateTime.UtcNow,
                Title = NotificationTypes.ReservationApproved,
                Type = NotificationTypes.ReservationApproved,
                Message = $"The Reservation{reservation.Id} Has been Appproved"
            }, $"staff-{reservation.RestaurantId}");

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return _responseHandler.BadRequest<Reservation>(
                    "Reservation already updated by another staff");
            }


            return _responseHandler.Success(reservation);
        }
        public async Task<Response<string>> RejectReservation(int reservationId)
        {
            var restaurantId = _currentUser.RestaurantId.Value;
            if (restaurantId == null)
                return _responseHandler.Unauthorized<string>();

            var reservation = await _unitOfWork.Repository<Reservation>()
                .GetEntityWithSpecAsync(new GetReservationForStaffSpecefication(reservationId, restaurantId));

            if (reservation == null)
                return _responseHandler.NotFound<string>("Reservation Not Found");

            // نتأكد إنه لسه pending
            if (reservation.Status != ReservationStatus.Pending)
                return _responseHandler.BadRequest<string>("Reservation already processed");

            // تغيير الحالة
            reservation.Status = ReservationStatus.Rejected;
            reservation.AssignedAt = DateTime.UtcNow;
            reservation.AssignedBy = _currentUser.UserName;

            await _notificationService.SendAsync(new NotificationDto
            {
                CreatedAt = DateTime.UtcNow,
                Title = NotificationTypes.ReservationRejected,
                Type = NotificationTypes.ReservationRejected,
                Message = $"Your Reservation {reservation.Id} Rejected"
            }, $"user-{reservation.UserId}");

            await _notificationService.SendAsync(new NotificationDto
            {
                CreatedAt = DateTime.UtcNow,
                Title = NotificationTypes.ReservationRejected,
                Type = NotificationTypes.ReservationRejected,
                Message = $"The Reservation {reservation.Id} Rejected"
            }, $"staff-{reservation.RestaurantId}");

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return _responseHandler.BadRequest<string>(
                    "Reservation already updated by another staff");
            }

            return _responseHandler.Success("Reservation Rejected Successfully");
        }
        public async Task<Response<string>> CompleteReservation(int reservationId)
        {
            if (_currentUser.RestaurantId == null)
                return _responseHandler.Unauthorized<string>();

            var restaurantId = _currentUser.RestaurantId.Value;

            var reservation = await _unitOfWork.Repository<Reservation>()
                .GetEntityWithSpecAsync(
                    new GetReservationForStaffSpecefication(
                        reservationId,
                        restaurantId));

            if (reservation == null)
            {
                return _responseHandler.NotFound<string>(
                    "Reservation Not Found");
            }

            // لازم يكون approved
            if (reservation.Status != ReservationStatus.Approved)
            {
                return _responseHandler.BadRequest<string>(
                    "Reservation is not approved");
            }

            reservation.Status = ReservationStatus.Completed;
            reservation.AssignedAt = DateTime.UtcNow;
            reservation.AssignedBy = _currentUser.UserName;

            await _notificationService.SendAsync(new NotificationDto
            {
                CreatedAt = DateTime.UtcNow,
                Title = NotificationTypes.ReservationCompleted,
                Type = NotificationTypes.ReservationCompleted,
                Message = $"Your Reservation {reservation.Id} Compelete Successfully"
            }, $"user-{reservation.UserId}");

            await _notificationService.SendAsync(new NotificationDto
            {
                CreatedAt = DateTime.UtcNow,
                Title = NotificationTypes.ReservationCompleted,
                Type = NotificationTypes.ReservationCompleted,
                Message = $"Reservation {reservation.Id} Compelete Successfully"
            }, $"user-{reservation.RestaurantId}");


            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return _responseHandler.BadRequest<string>(
                    "Reservation already updated by another staff");
            }


            return _responseHandler.Success(
                "Reservation completed successfully");
        }

        public async Task AutoCancel()
        {
            var pendingReservations = await _unitOfWork.Repository<Reservation>()
                .GetTable()
                .Where(r =>

                    r.Status == ReservationStatus.Pending &&

                    r.CreatedAt.AddMinutes(15) <= DateTime.UtcNow
                )
                .ToListAsync();

            foreach (var reservation in pendingReservations)
            {
                reservation.Status = ReservationStatus.Cancelled;
            }

            await _unitOfWork.CompleteAsync();
        }
        public async Task AutoCompelete()
        {
            var ApprovedReservations = await _unitOfWork.Repository<Reservation>()
                .GetTable()
                .Where(r =>

                    r.Status == ReservationStatus.Approved &&

                    r.EndDateTime <= DateTime.UtcNow
                )
                .ToListAsync();

            foreach (var reservation in ApprovedReservations)
            {
                reservation.Status = ReservationStatus.Completed;
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task RememberUser()
        {
            var reservations = await _unitOfWork.Repository<Reservation>()
                .GetTable()
                .Where(r => r.ReminderSend == true)
                .ToListAsync();

            foreach (var reservation in reservations)
            {
                var user = await _userManager.FindByIdAsync(reservation.UserId);

                BackgroundJob.Enqueue<IEmailSender>(x =>
                 x.SendEmailAsync(
                     user.Email,
                     "Reservation Reminder",
                     $@"Your reservation will start at<b>{reservation.StartDateTime}</b>in 30 minutes."));
                reservation.ReminderSend = false;
            }
        }
    }

}
