using Book.Data.Entities;
using Booking.Service.Abstracts;
using Booking.Service.Base;
using Microsoft.AspNetCore.SignalR;
using Restaurant.Data.Dtos;
using Talabat.Core.Repo.Contarct;

namespace Booking.Service.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<ReservationHub> _hub;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(
            IHubContext<ReservationHub> hub,
            IUnitOfWork unitOfWork)
        {
            _hub = hub;
            _unitOfWork = unitOfWork;
        }

        public async Task SendAsync(NotificationDto dto, string targetGroup)
        {
            var notification = new Notification
            {
                TargetGroup = targetGroup,
                Title = dto.Title,
                Message = dto.Message,
                Type = dto.Type,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Notification>().AddAsync(notification);

            await _unitOfWork.CompleteAsync();

            await _hub.Clients.Group(targetGroup)
                .SendAsync("ReceiveNotification", new NotificationDto
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Message = notification.Message,
                    Type = notification.Type,
                    CreatedAt = notification.CreatedAt
                });
        }

    }
}
