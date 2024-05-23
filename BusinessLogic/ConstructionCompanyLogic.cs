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

            constructionCompany.SelfValidate();
            ConstructionCompany existingCompanyByAdmin = _constructionCompanyRepository.GetConstructionCompanyByAdmin(constructionCompany.ConstructionCompanyAdmin.Id);
            
            if (existingCompanyByAdmin != null) { 
                throw new InvalidOperationException("Only one construction company can be created per user.");
            }

            ConstructionCompany existingCompany = _constructionCompanyRepository.GetConstructionCompanyByName(constructionCompany.Name);
            
            if (existingCompany != null)
            {
                throw new InvalidOperationException("Only one construction company can be created.");
            }

            return _constructionCompanyRepository.CreateConstructionCompany(constructionCompany);
        }

        public ConstructionCompany UpdateConstructionCompanyName(string newName, User constructionCompanyAdmin)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new EmptyFieldException("New name can't be null or empty");
            }

            if (constructionCompanyAdmin == null)
            {
                throw new ObjectNotFoundException("Construction Company Administrator not found");
            }

            ConstructionCompany companyToUpdate = _constructionCompanyRepository.GetConstructionCompanyByAdmin(constructionCompanyAdmin.Id);
            
            if (companyToUpdate == null)
            {
                throw new InvalidOperationException("Construction company not found.");
            }

            ConstructionCompany existingCompany = _constructionCompanyRepository.GetConstructionCompanyByName(newName);
           
            if (existingCompany != null)
            {
                throw new ObjectAlreadyExistsException("Construction company with that name already exists.");
            }

            companyToUpdate.Name = newName;

            return _constructionCompanyRepository.UpdateConstructionCompany(companyToUpdate);
        }
    }
}