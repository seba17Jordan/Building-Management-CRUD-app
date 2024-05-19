using CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ConstructionCompany
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public User ConstructionCompanyAdmin { get; set; }

        public ConstructionCompany() { }

        public ConstructionCompany(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public void SelfValidate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new EmptyFieldException("There are empty fields");
            }
        }

        public override bool Equals(object constructionCompany)
        {
            if (constructionCompany == null || GetType() != constructionCompany.GetType())
            {
                return false;
            }
            ConstructionCompany secondConstructionCompany = (ConstructionCompany)constructionCompany;

            return Name == secondConstructionCompany.Name;
        }
    }
}
