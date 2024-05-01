using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class MaintenanceReportResponse
    {
        public string Name { get; set; }
        public int PendingRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int DoneRequests { get; set; }
        public string AverageTime { get; set; }

        public MaintenanceReportResponse((string, int, int, int, string) info)
        {
            Name = info.Item1;
            PendingRequests = info.Item2;
            InProgressRequests = info.Item3;
            DoneRequests = info.Item4;
            AverageTime = info.Item5;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            MaintenanceReportResponse other = (MaintenanceReportResponse)obj;
            return Name == other.Name &&
                PendingRequests == other.PendingRequests &&
                InProgressRequests == other.InProgressRequests &&
                DoneRequests == other.DoneRequests &&
                AverageTime == other.AverageTime;
        }
    }
}