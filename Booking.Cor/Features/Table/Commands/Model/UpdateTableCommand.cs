using MediatR;
using Restaurant.Cor.Features.Table.Queries.Result;
using Restaurant.Service;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Cor.Features.Table.Commands.Model
{
    public class UpdateTableCommand : IRequest<Response<GetTableDto>>
    {
        //[Required]
        //public int RestaurantId { get; set; }
        [Required]
        public int Capacity { get; set; }
    }
}
