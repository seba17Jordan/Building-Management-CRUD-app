﻿using Domain;
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
        private Guid? _currentUser;

        public SessionService(ISessionRepository sessionRepository, IUserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }
        public Guid Authenticate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Invalid data");
            }

            var user = _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                throw new ArgumentException("User not found");
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

        public Guid GetUserByToken(Guid token)
        {
            if (_currentUser != null)
                return (Guid)_currentUser;

            if (token == null)
                throw new ArgumentException("No authorization token");

            var currentSession = _sessionRepository.GetSessionByToken(token);

            if (currentSession != null)
                _currentUser = currentSession.UserId;

            return (Guid)_currentUser;
        }
    }
}
