using CustomExceptions;
using Domain.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string? LastName { get; set; }
        public Roles Role { get; set; }

        public User() { 
            Id = Guid.NewGuid();
        }

        public void SelfValidate()
        {
            if(string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Name))
            {
                throw new EmptyFieldException("There are empty fields");
            }

            if (Password.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long");
            }

            if (!IsValidEmail(Email))
            {
                throw new ArgumentException("Invalid email format", nameof(Email));
            }
        }

        public void SelfValidateData()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                throw new EmptyFieldException("There are empty fields");
            }

            if (Password.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long");
            }

            if (!IsValidEmail(Email))
            {
                throw new ArgumentException("Invalid email format", nameof(Email));
            }
        }

        private bool IsValidEmail(string email)
        {
            string emailRegexPattern = @"^\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b$";
            return Regex.IsMatch(email, emailRegexPattern, RegexOptions.IgnoreCase);
        }
    }
}
