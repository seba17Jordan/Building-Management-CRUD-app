using Domain;

namespace IDataAccess
{
    public interface IUserRepository
    {
        User CreateUser(User user);
        IEnumerable<User> GetManagers();
        User GetUserByEmail(string email);
        User? GetUserById(Guid currentUserId);
        User GetUserByName(string? maintenanceName);
        bool UserExists(Func<User,bool> predicate);
    }
}
