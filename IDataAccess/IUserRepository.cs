using Domain;

namespace IDataAccess
{
    public interface IUserRepository
    {
       User CreateUser(User user);
       bool UserExists(Func<User,bool> predicate);
       //User GetUserById(Guid id);
       //User UpdateUser(User user);
    }
}
