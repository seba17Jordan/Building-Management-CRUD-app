using Domain;
using IDataAccess;

namespace DataAccess
{
    public class UserRepository : IUserRepository
    {
        public User CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(Func<User, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
