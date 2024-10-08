﻿using CustomExceptions;

namespace Domain
{
    public class Owner
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Owner() { }

        public void SelfValidate()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Email))
            {
                throw new EmptyFieldException("Owner fields are empty");
            }
        }

        public override bool Equals(object owner)
        {
            if (owner == null || GetType() != owner.GetType())
            {
                return false;
            }

            Owner secondOwner = (Owner)owner;

            // Comparación de propiedades
            return Name == secondOwner.Name &&
                   LastName == secondOwner.LastName &&
                   Email == secondOwner.Email;
        }
    }
}