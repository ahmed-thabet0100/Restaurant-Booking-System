namespace Restaurant.Cor.Features.Table.Queries.Result
{
    public class GetTableDto
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }

        public int RestaurantId { get; set; }

        public bool IsAvailable { get; set; } = true;

    }
}
