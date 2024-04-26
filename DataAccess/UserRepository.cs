using Domain;
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

        public bool UserExists(Func<User, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
