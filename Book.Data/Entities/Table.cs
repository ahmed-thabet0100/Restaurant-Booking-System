
namespace Book.Data.Entities
{
    public class Table
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }


        public static explicit operator Table(Task<Table> v)
        {
            throw new NotImplementedException();
        }
    }
}