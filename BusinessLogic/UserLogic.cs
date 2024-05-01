using Domain;
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

            if (_userRepository.UserExists(GetUsersByMail(user.Email)))
            {
                throw new ArgumentException("User already exists");
            }
            if (!IsValidEmail(user.Email))
            {
                throw new ArgumentException("Invalid email format", nameof(user.Email));
            }

            if (string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Name) ||
                string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("Invalid data");
            }

            return _userRepository.CreateUser(user);
        }

        private Func<User, bool> GetUsersByMail(string email)
        {
            return (User u) => email == "" || u.Email == email;
        }

        private bool IsValidEmail(string email)
        {
            string emailRegexPattern = @"^\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b$";
            return Regex.IsMatch(email, emailRegexPattern, RegexOptions.IgnoreCase);
        }
    }
}
