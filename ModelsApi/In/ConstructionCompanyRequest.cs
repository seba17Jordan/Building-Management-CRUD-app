using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.In
{
    public class ConstructionCompanyRequest
    {
        public string Name { get; set; }

        public ConstructionCompany ToEntity()
        {
            return new ConstructionCompany(Name);
        }
        
    }
}
