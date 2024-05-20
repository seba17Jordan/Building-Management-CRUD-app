using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class ConstructionCompanyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ConstructionCompanyResponse(ConstructionCompany constructionCompany)
        {
            Id = constructionCompany.Id;
            Name = constructionCompany.Name;
        }
    }
}

