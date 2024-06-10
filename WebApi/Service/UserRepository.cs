namespace WebApi.Service
{
    public class UserRepository : IUserRepository
    {
        public bool AuthorizeUser(string username, string password)
        {
            return true;
        }
    }
}
