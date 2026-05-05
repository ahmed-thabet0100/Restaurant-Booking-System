using Restaurant.Cor.Features.Restaurant.Commands.Model;
using Restaurant.Cor.Features.Restaurant.Queries.Result;

namespace Restaurant.Cor.Mapping.Student
{
    public partial class RestaurantProfile
    {
        public void UpdateRestaurantCommandMapping()
        {
            CreateMap<UpdateRestaurantCommand, Book.Data.Entities.Restaurant>().ReverseMap();
            CreateMap<ReturnRestaurantQuery, Book.Data.Entities.Restaurant>().ReverseMap();
        }
    }
}
