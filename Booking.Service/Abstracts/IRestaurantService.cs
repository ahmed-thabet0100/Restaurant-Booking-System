using Booking.Data.Entities;
using Booking.Infra.Specifications.ReviewSpecifications;
using Restaurant.Infra.Specifications.RestaurantSpecifications;

namespace Restaurant.Service.Abstracts
{
    public interface IRestaurantService
    {

        public Task<Book.Data.Entities.Restaurant> AddRestaurantAsync(Book.Data.Entities.Restaurant entity);
        public Task<(IReadOnlyList<Book.Data.Entities.Restaurant> Data, int Count)> GetAllRestaurantsAsync(RestaurantSpecParams specParams);
        public Task<Book.Data.Entities.Restaurant>? GetRestaurantAsyncByIdAsync(int Id);
        public Task DeleteRestaurantAsync();
        public Task<Book.Data.Entities.Restaurant> UpdateRestaurantAsync(Book.Data.Entities.Restaurant entity);
        public Task<Review> CreateReviewAsync(Review review);
        public Task<(IReadOnlyList<Review>, int Count)> GetReviewAsync(ReviewSpecParams specParams);
        public Task UpdateAllRestaurants();


    }
}
