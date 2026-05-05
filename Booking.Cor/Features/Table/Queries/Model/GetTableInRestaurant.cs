using MediatR;
using Restaurant.Cor.Features.Table.Queries.Result;
using Restaurant.Service;

namespace Restaurant.Cor.Features.Table.Queries.Model
{
    public class GetTableInRestaurant : IRequest<Response<GetTableDto>>
    {
        public GetTableInRestaurant(int tableNum/*, int restaurantId*/)
        {
            TableNum = tableNum;
            //RestaurantId = restaurantId;
        }

        public int TableNum { get; set; }
        //public int RestaurantId { get; set; }
    }
}
