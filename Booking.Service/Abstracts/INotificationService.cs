using Restaurant.Data.Dtos;

namespace Booking.Service.Abstracts
{
    public interface INotificationService
    {
        Task SendAsync(NotificationDto dto, string targetGroup);
    }
}
