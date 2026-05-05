namespace Restaurant.Service.Abstracts
{
    public interface IInvetationAdminSender
    {
        Task<Response<string>> InvideAdmin(string Email);
    }
}
