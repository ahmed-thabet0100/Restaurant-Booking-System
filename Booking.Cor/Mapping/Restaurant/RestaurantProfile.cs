using AutoMapper;

namespace Restaurant.Cor.Mapping.Student
{
    public partial class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            // Commands
            AddRestaurantCommandMapping();
            UpdateRestaurantCommandMapping();
            RemoveRestaurantCommandMapping();
            AddReviewCommandMapping();
            //Queries
            ReturnRestaurantMappingQuery();
            GetReviewsQueryMapping();
        }
    }
}
