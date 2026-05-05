using MediatR;
using Restaurant.Service;

namespace Restaurant.Cor.Features.Table.Commands.Model
{
    public class DeleteTableCommand : IRequest<Response<string>>
    {
        public DeleteTableCommand(int tableNum /* int restaurantId*/)
        {
            TableNum = tableNum;
            //RestaurantId = restaurantId;
        }

        public int TableNum { get; set; }
        //public int RestaurantId { get; set; }
    }
}
