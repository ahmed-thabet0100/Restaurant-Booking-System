using MediatR;
using Restaurant.Cor.Features.Restaurant.Queries.Result;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Cor.Features.Restaurant.Commands.Model
{
    public class AddRestaurandCommand : IRequest<Response<ReturnRestaurantQuery>>
    {
        [Required(ErrorMessage = "Restaurant name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone]
        public string Phone { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(200)]
        public string OpeningHours { get; set; }

    }
}
