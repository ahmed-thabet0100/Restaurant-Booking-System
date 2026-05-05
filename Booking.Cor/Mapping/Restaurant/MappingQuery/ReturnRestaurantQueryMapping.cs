using Restaurant.Cor.Features.Restaurant.Queries.Result;

namespace Restaurant.Cor.Mapping.Student
{
    public partial class RestaurantProfile
    {
        public void ReturnRestaurantMappingQuery()
        {
            CreateMap<Book.Data.Entities.Restaurant, ReturnRestaurantQuery>().ReverseMap();
        }
    }
}
