using Domain;
using IDataAccess;
using LogicInterface;

namespace BusinessLogic
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _userRepository;
        public UserLogic(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public User CreateUser(User user) { 
            throw new NotImplementedException();
        }
    }
}
