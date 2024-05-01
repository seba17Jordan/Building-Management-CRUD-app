using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterface
{
    public interface IReportLogic
    {
        IEnumerable<(string, int, int, int, string)> GetMaintenanceReport(string buildingName, Guid id, Guid? maintenance);
        public IEnumerable<(string, int, int, int)> GetReport(Guid userId, string param);
    }
}
