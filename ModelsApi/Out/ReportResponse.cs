using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class ReportResponse
    {
        public string Name { get; set; }
        public int PendingRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int DoneRequests { get; set; }

        public ReportResponse((string, int, int, int) info)
        {
            Name = info.Item1;
            PendingRequests = info.Item2;
            InProgressRequests = info.Item3;
            DoneRequests = info.Item4;
        }
    }
}