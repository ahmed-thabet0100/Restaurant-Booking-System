namespace Restaurant.Infra.Abstracts
{
    public interface IRestaurantRepo
    {
        public Task<bool> IsNameExistAsync(string Name);
    }
}
