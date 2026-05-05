namespace Restaurant.Service.Abstracts
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string Role { get; }
        int? RestaurantId { get; }
        string? Email { get; }
        string UserName { get; }
    }
}
