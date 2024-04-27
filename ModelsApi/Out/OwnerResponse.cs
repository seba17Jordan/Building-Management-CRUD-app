using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class OwnerResponse
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public OwnerResponse(Owner owner)
        {
            Name = owner.Name;
            LastName = owner.LastName;
            Email = owner.Email;
        }

        public override bool Equals(object owner)
        {
            if (owner == null || GetType() != owner.GetType())
            {
                return false;
            }

            OwnerResponse secondOwner = (OwnerResponse)owner;

            // Comparación de propiedades
            return Name == secondOwner.Name &&
                   LastName == secondOwner.LastName &&
                   Email == secondOwner.Email;
        }
    }
}
