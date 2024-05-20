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
            throw new NotImplementedException();
        }
    }
}