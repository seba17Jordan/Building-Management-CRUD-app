using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterface
{
    public interface IBuildingLogic
    {
        Building CreateBuilding(Building building);
        void DeleteBuildingById(Guid id, Guid managerId);
        Building UpdateBuildingById(Guid id, Building building, Guid managerId);
        Building GetBuildingById(Guid id);
        Building ModifyBuildingManager(Guid buildingId, Guid newManagerId, Guid constructionCompanyAdminId);
    }
}
