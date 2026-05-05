using Booking.Data.Entities;
using Booking.Infra.Specifications.RestaurantSpecifications;
using Booking.Infra.Specifications.ReviewSpecifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Entities.Identity;
using Restaurant.Infra.Abstracts;
using Restaurant.Infra.Data;
using Restaurant.Infra.Specifications.RestaurantSpecifications;
using Restaurant.Service.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Talabat.Core.Repo.Contarct;

namespace Restaurant.Service.Implementation
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRestaurantRepo _Repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUser;
        private readonly ResponseHandler _responseHandler;

        public RestaurantService(IUnitOfWork unitOfWork,
            IRestaurantRepo Repo,
            AppDbContext dbContext,
            UserManager<AppUser> userManager,
            ICurrentUserService currentUser,
            ResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _Repo = Repo;
            _userManager = userManager;
            _currentUser = currentUser;
            _responseHandler = responseHandler;
        }

        public int GetRestaurantId()
        {
            if (!_currentUser.RestaurantId.HasValue)
                throw new UnauthorizedAccessException("User has no restaurant");

            return _currentUser.RestaurantId.Value;
        }

        public async Task<Book.Data.Entities.Restaurant> AddRestaurantAsync(Book.Data.Entities.Restaurant entity)
        {
            // 🔹 check if user already has restaurant
            var exists = await _unitOfWork.Repository<Book.Data.Entities.Restaurant>()
                .GetEntityWithSpecAsync(new GetRestaurantOwner(_currentUser.UserId));

            if (exists != null)
                throw new Exception("You already have a restaurant");

            // 🔹 assign owner
            entity.OwnerId = _currentUser.UserId;

            await _unitOfWork.Repository<Book.Data.Entities.Restaurant>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            // 🔥 update user
            var user = await _userManager.FindByIdAsync(_currentUser.UserId);
            user.RestaurantId = entity.Id;
            await _userManager.UpdateAsync(user);


            return entity;
        }
        public async Task DeleteRestaurantAsync()
        {
            var restaurantId = GetRestaurantId();
            var restaurant = await _unitOfWork.Repository<Book.Data.Entities.Restaurant>().GetAsync(restaurantId);

            if (restaurant == null)
                throw new KeyNotFoundException("Restaurant not found");

            // 🔹 Get user(s) linked to this restaurant
            var users = await _userManager.Users
                .Where(u => u.RestaurantId == restaurantId)
                .ToListAsync();

            // 🔹 remove relation
            foreach (var user in users)
            {
                user.RestaurantId = null;
                await _userManager.UpdateAsync(user);
            }

            // 🔹 delete restaurant
            await _unitOfWork.Repository<Book.Data.Entities.Restaurant>().Remove(restaurant);
            await _unitOfWork.CompleteAsync();
        }
        public async Task<(IReadOnlyList<Book.Data.Entities.Restaurant> Data, int Count)>
            GetAllRestaurantsAsync(RestaurantSpecParams specParams)
        {
            var spec = new RestaurantSpecification(specParams);
            var countSpec = new RestaurantForCountSpecification(specParams);

            var repo = _unitOfWork.Repository<Book.Data.Entities.Restaurant>();

            var totalItems = await repo.CountAsync(countSpec);
            var restaurants = await repo.GetAllEntitiesWithSpecAsync(spec);

            return (restaurants, totalItems);
        }
        public async Task<Book.Data.Entities.Restaurant?> GetRestaurantAsyncByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Book.Data.Entities.Restaurant>().GetAsync(id);
        }
        public async Task<Book.Data.Entities.Restaurant?> UpdateRestaurantAsync(Book.Data.Entities.Restaurant entity)
        {
            var restaurant = await _unitOfWork.Repository<Book.Data.Entities.Restaurant>()
                .GetAsync(GetRestaurantId());

            if (restaurant == null)
                return null;

            restaurant.Address = entity.Address;
            restaurant.Phone = entity.Phone;
            restaurant.OpeningHours = entity.OpeningHours;
            restaurant.IsActive = entity.IsActive;

            await _unitOfWork.CompleteAsync();

            return restaurant;
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            if (_currentUser?.UserId == null)
                throw new UnauthorizedAccessException();
            review.UserId = _currentUser.UserId;
            review.UserName = _currentUser.UserName;

            if (review.Rating < 1 || review.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5");

            var existingReview = await _unitOfWork.Repository<Review>()
                .GetTableAsNoTracking()
                .FirstOrDefaultAsync(r =>
                    r.RestaurantId == review.RestaurantId &&
                    r.UserId == review.UserId);

            if (existingReview != null)
                throw new ValidationException("You already reviewed this restaurant");

            var restaurant = await _unitOfWork.Repository<Book.Data.Entities.Restaurant>()
                .GetAsync(review.RestaurantId);

            if (restaurant == null)
                throw new KeyNotFoundException("Restaurant not found");

            await _unitOfWork.Repository<Review>().AddAsync(review);
            restaurant.NeedsRatingUpdate = true;

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("You already reviewed this restaurant");
            }
            return review;
        }
        public async Task<(IReadOnlyList<Review>, int Count)> GetReviewAsync(ReviewSpecParams specParams)
        {
            var repo = _unitOfWork.Repository<Review>();

            var reviews = await repo.GetAllEntitiesWithSpecAsync(
                new AllReviewsInRestaurant(specParams))
                ?? new List<Review>();

            var count = await repo.CountAsync(
                new ReviewForCountSpecification(specParams));

            return (reviews, count);
        }
        public async Task UpdateAllRestaurants()
        {
            var restaurants = await _unitOfWork.Repository<Book.Data.Entities.Restaurant>()
                .GetTable()
                .Where(r => r.NeedsRatingUpdate)
                .ToListAsync();

            foreach (var restaurant in restaurants)
            {
                var data = await _unitOfWork.Repository<Review>()
                    .GetTable()
                    .Where(r => r.RestaurantId == restaurant.Id)
                    .GroupBy(r => r.RestaurantId)
                    .Select(g => new
                    {
                        Avg = g.Average(x => x.Rating),
                        Count = g.Count()
                    })
                    .FirstOrDefaultAsync();

                if (data != null)
                {
                    restaurant.AverageRating = data.Avg;
                    restaurant.ReviewsCount = data.Count;
                }

                restaurant.NeedsRatingUpdate = false;
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}
