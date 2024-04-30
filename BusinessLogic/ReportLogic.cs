using LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class ReportLogic : IReportLogic
    {
        public IEnumerable<(string, int, int, int)> GetReport(Guid userId, string param) { 
            throw new NotImplementedException();
        }
    }
}
