using MediatR;
using Restaurant.Cor.Features.Restaurant.Queries.Result;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Cor.Features.Restaurant.Commands.Model
{
    public class UpdateRestaurantCommand : IRequest<Response<ReturnRestaurantQuery>>
    {

        [MaxLength(200)]
        public string Address { get; set; }

        [Phone]
        public string Phone { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(200)]
        public string OpeningHours { get; set; }


    }
}
