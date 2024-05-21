using CustomExceptions;
using Domain;
using IDataAccess;
using LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class ConstructionCompanyLogic : IConstructionCompanyLogic
    {
        private readonly IConstructionCompanyRepository _constructionCompanyRepository;
        public ConstructionCompanyLogic(IConstructionCompanyRepository constructionCompanyRepository)
        {
            _constructionCompanyRepository = constructionCompanyRepository;
        }

        public ConstructionCompany CreateConstructionCompany(ConstructionCompany constructionCompany)
        {
            if (constructionCompany == null)
            {
                throw new ArgumentNullException(nameof(constructionCompany), "ConstructionCompany can't be null");
            }

            if(string.IsNullOrWhiteSpace(constructionCompany.Name))
            {
                throw new ArgumentNullException(nameof(constructionCompany.Name), "ConstructionCompany name can't be null or empty");
            }

            constructionCompany.SelfValidate();
            var existingCompanyByAdmin = _constructionCompanyRepository.GetConstructionCompanyByAdmin(constructionCompany.ConstructionCompanyAdmin.Id);
            if (existingCompanyByAdmin == true) { 
                throw new InvalidOperationException("Only one construction company can be created per user.");
            }


            var existingCompany = _constructionCompanyRepository.GetConstructionCompanyByName(constructionCompany.Name);
            if (existingCompany != null)
            {
                throw new InvalidOperationException("Only one construction company can be created.");
            }

            return _constructionCompanyRepository.CreateConstructionCompany(constructionCompany);
        }
    }
}