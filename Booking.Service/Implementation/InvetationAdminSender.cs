using Hangfire;
using Restaurant.Data.Entities;
using Restaurant.Infra.Abstracts;
using Restaurant.Service.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Talabat.Core.Repo.Contarct;

namespace Restaurant.Service.Implementation
{
    public class InvetationAdminSender : IInvetationAdminSender
    {
        private readonly IEmailSender _emailSender;
        private readonly IInvetationRepo _invetationRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHandler _responseHandler;
        private readonly ICurrentUserService _currentUser;

        public InvetationAdminSender(IEmailSender emailSender,
            IInvetationRepo invetationRepo
            , IUnitOfWork unitOfWork,
            ResponseHandler responseHandler,
            ICurrentUserService currentUser
            )
        {
            _emailSender = emailSender;
            _invetationRepo = invetationRepo;
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _currentUser = currentUser;
        }
        public int GetRestaurantId()
        {
            if (!_currentUser.RestaurantId.HasValue)
                throw new UnauthorizedAccessException("User has no restaurant");

            return _currentUser.RestaurantId.Value;
        }

        public async Task<Response<string>> InvideAdmin(string email)
        {
            // 🔹 Validation
            if (string.IsNullOrWhiteSpace(email))
                return _responseHandler.BadRequest<string>("Email is required");

            if (!new EmailAddressAttribute().IsValid(email))
                return _responseHandler.BadRequest<string>("Invalid email format");
            var restaurantId = GetRestaurantId();

            var restaurant = await _unitOfWork.Repository<Book.Data.Entities.Restaurant>().GetAsync(restaurantId);

            if (restaurant == null)
                return _responseHandler.NotFound<string>("Restaurant not found");

            // 🔹 Check existing active invitation
            var existingInvite = await _invetationRepo
                .GetActiveInvitationAsync(email, restaurantId);

            if (existingInvite != null && !existingInvite.IsUsed && existingInvite.ExpiryDate > DateTime.UtcNow)
                return _responseHandler.Conflict<string>("Invitation already sent. Please wait until it expires");

            // 🔹 Generate OTP
            var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            var invitation = new RestaurantAdminInvitation
            {
                Email = email,
                RestaurantId = restaurantId,
                Code = code,
                ExpiryDate = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            await _invetationRepo.AddAsync(invitation);
            await _unitOfWork.CompleteAsync();

            // 🔹 Send Email (Background)
            BackgroundJob.Enqueue<IEmailSender>(x =>
                x.SendEmailAsync(
                    email,
                    "Admin Invitation",
                    $"Your invitation code is <b>{code}</b> and valid for 10 minutes."
                )
            );

            return _responseHandler.Created<string>("Invitation sent successfully");
        }
    }
}
