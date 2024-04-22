﻿using Domain;
using IDataAccess;
using LogicInterface;
using System.Globalization;

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
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user), "User can't be null");
                }

                if (string.IsNullOrWhiteSpace(user.Email) ||
                    string.IsNullOrWhiteSpace(user.Name) ||
                    string.IsNullOrWhiteSpace(user.Password) ||
                    string.IsNullOrWhiteSpace(user.Role))
                {
                    throw new ArgumentException("Invalid data");
                }

                _userRepository.CreateUser(user);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create user", ex);
            }
        }
    }
}
