namespace WebApi.Service
{
    public interface IUserRepository
    {
        bool AuthorizeUser(string username, string password);
    }
}
