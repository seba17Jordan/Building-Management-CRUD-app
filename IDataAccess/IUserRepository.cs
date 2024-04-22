using Domain;

namespace IDataAccess
{
    public interface IUserRepository
    {
       User CreateUser(User user);
       User GetUserById(Guid id);
       User UpdateUser(User user);
    }
}
