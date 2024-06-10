using ModelsApi.In;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportersInterface
{
    public interface IBuildingService
    {
        void CreateBuilding(BuildingRequest buildingRequest, string contructionCompanyAdminToken);
    }
}
