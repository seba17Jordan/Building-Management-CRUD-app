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
    public class Invitation { 
        public Status State { get; set; }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Roles Role { get; set; }

        public Invitation() { }

        public Invitation(string email, string name, DateTime expirationDate,Roles role)
        {
            Email = email;
            Name = name;
            ExpirationDate = expirationDate;
            Role = role;
            State = Status.Pending;
        }

        public void SelfValidate()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Name))
            {
                throw new EmptyFieldException("There are empty fields");
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
