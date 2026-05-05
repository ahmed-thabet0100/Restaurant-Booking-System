namespace Restaurant.Infra.Specifications.RestaurantSpecifications
{
    public class RestaurantSpecParams
    {
        private const int MaxPageSize = 50;

        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? Search { get; set; }

        // sort = capacityAsc | capacityDesc | numberAsc | numberDesc
        public string? Sort { get; set; }

    }
}