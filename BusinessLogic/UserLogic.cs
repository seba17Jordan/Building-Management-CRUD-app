using CustomExceptions;
using Domain;
using Domain.@enum;
using IDataAccess;
using LogicInterface;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BusinessLogic
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _userRepository;
        public UserLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User CreateUser(User user) {
           
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User can't be null");
            }

            user.SelfValidate();

            if (_userRepository.UserExists(GetUsersByMail(user.Email)))
            {
                throw new ObjectAlreadyExistsException("Email already exists");
            }

            return _userRepository.CreateUser(user);
        }

        public IEnumerable<User> GetAllManagers()
        {
            return _userRepository.GetManagers();
        }

        private Func<User, bool> GetUsersByMail(string email)
        {
            return (User u) => email == "" || u.Email == email;
        }

        public IEnumerable<User> GetAllMaintenancePersons()
        {
            return _userRepository.GetAllMaintenance();
        }
    }
}
