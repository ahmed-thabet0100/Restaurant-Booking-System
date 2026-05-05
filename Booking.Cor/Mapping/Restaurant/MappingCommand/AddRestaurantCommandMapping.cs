using Restaurant.Cor.Features.Restaurant.Commands.Model;

namespace Restaurant.Cor.Mapping.Student
{
    public partial class RestaurantProfile
    {
        public void AddRestaurantCommandMapping()
        {
            CreateMap<AddRestaurandCommand, Book.Data.Entities.Restaurant>().ReverseMap();
        }
    }
}
