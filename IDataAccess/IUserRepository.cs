using Domain;

namespace IDataAccess
{
    public interface IUserRepository
    {
        User CreateUser(User user);
        User GetUserByEmail(string email);
        User? GetUserById(Guid currentUserId);
        bool UserExists(Func<User,bool> predicate);
    }
}
