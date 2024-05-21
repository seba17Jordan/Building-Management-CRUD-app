using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDataAccess
{
    public interface IConstructionCompanyRepository
    {
        ConstructionCompany GetConstructionCompanyByName(string name);

        ConstructionCompany CreateConstructionCompany(ConstructionCompany constructionCompany);

        bool GetConstructionCompanyByAdmin(Guid id);
    }
}
