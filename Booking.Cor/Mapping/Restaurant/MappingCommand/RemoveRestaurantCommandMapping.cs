namespace Restaurant.Cor.Mapping.Student
{
    public partial class RestaurantProfile
    {
        public void RemoveRestaurantCommandMapping()
        {
            CreateMap<Features.Restaurant.Commands.Model.RemoveRestaurantCommand, Book.Data.Entities.Restaurant>();
        }
    }
}
