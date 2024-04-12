using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi
{
    public class OwnerResponse
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public OwnerResponse(Owner owner)
        {
            this.Name = owner.Name;
            this.LastName = owner.LastName;
            this.Email = owner.Email;
        }
    }
}
