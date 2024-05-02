using CustomExceptions;
using Domain;
using IDataAccess;
using LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace BusinessLogic
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        private User? _currentUser;

        public SessionService(ISessionRepository sessionRepository, IUserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }
        public Guid Authenticate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new EmptyFieldException("There are empty fields");
            }

            var user = _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                throw new ArgumentNullException("User not found");
            }

            if (user.Password != password)
            {
                throw new ArgumentException("Invalid password");
            }

            var actualSession = new Session() {
                User = user 
            };

            _sessionRepository.Insert(actualSession);
            _sessionRepository.Save();

            return actualSession.Token;
        }

        public User GetUserByToken(Guid token)
        {
            if (_currentUser != null)
            {
                return _currentUser;
            }

            if (token == null)
            {
                throw new ArgumentNullException("No authorization token");
            }

            var currentSession = _sessionRepository.GetSessionByToken(token);

            if (currentSession != null)
            {
                Guid _currentUserId = currentSession.UserId;
                _currentUser = _userRepository.GetUserById(_currentUserId);
            }
            else
            {
                throw new ArgumentNullException("No session found");
            }

            return _currentUser;
        }

        public void Logout(Guid token)
        {
            if (token == null)
                throw new ArgumentException("No authorization token");

            var currentSession = _sessionRepository.GetSessionByToken(token);

            if (currentSession != null)
            {
                _sessionRepository.Delete(currentSession);
                _sessionRepository.Save();
            }
        }
    }
}
