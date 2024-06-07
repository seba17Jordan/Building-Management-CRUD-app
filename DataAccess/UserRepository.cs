using Domain;
using Domain.@enum;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(DbContext context)
        {
            _context = context;
        }
        public User CreateUser(User user)
        {
            _context.Set<User>().Add(user);
            _context.SaveChanges();
            return user;
        }

        public IEnumerable<User> GetManagers()
        {
            return _context.Set<User>().Where(u => u.Role == Roles.Manager);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Set<User>().FirstOrDefault(u => u.Email == email);
        }

        public User? GetUserById(Guid currentUserId)
        {
            return _context.Set<User>().FirstOrDefault(u => u.Id == currentUserId);
        }

        public User GetUserByName(string? name)
        {
            return _context.Set<User>().FirstOrDefault(u => u.Name == name);
        }

        public bool UserExists(Func<User, bool> predicate)
        {
            return _context.Set<User>().Any(predicate);
        }
    }
}
