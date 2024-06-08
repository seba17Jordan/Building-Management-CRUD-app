using Domain.@enum;
using Domain;
using IDataAccess;
using LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomExceptions;

namespace BusinessLogic
{
    public class ReportLogic : IReportLogic
    {
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IUserRepository _userRepository;

        public ReportLogic(IServiceRequestRepository serviceRequestRepository,
                           IBuildingRepository buildingRepository, IUserRepository userRepository)
        {
            _serviceRequestRepository = serviceRequestRepository;
            _buildingRepository = buildingRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<(string, int, int, int, string)> GetMaintenanceReport(string buildingName, Guid Managerid, string? maintenanceName)
        {
            Building currentBuilding = _buildingRepository.GetBuildingByName(buildingName); 

            if (currentBuilding == null)
            {
                throw new ObjectNotFoundException("Building not found.");
            }

            if(currentBuilding.Manager.Id != Managerid)
            {
                throw new InvalidOperationException("Manager does not have access to this building.");
            }

            IEnumerable<ServiceRequest> serviceRequests = _serviceRequestRepository.GetServiceRequestsByBuilding(currentBuilding.Id);

            if (!string.IsNullOrEmpty(maintenanceName))
            {
                User maintenancePerson = _userRepository.GetUserByName(maintenanceName);

                if (maintenancePerson == null)
                {
                    throw new ObjectNotFoundException("Maintenance person not found.");
                }
                serviceRequests = serviceRequests.Where(sr => sr.MaintenanceId == maintenancePerson.Id); 
            }

            var groupedRequests = serviceRequests.GroupBy(sr => sr.MaintenanceId);

            List<(string, int, int, int, string)> reportList = new List<(string, int, int, int, string)>();

            
            foreach (var serviceRequestGroup in groupedRequests)
            {
                double averageCompletionTime = 0;
                averageCompletionTime = CalculateAverageCompletionTime(serviceRequestGroup);

                Guid MaintenancePerson = (Guid)serviceRequestGroup.Key;
                string maintenancePersonName = _userRepository.GetUserById(MaintenancePerson).Name;
                int OpenRequests = serviceRequestGroup.Count(sr => sr.Status == ServiceRequestStatus.Open);
                int AttendingRequests = serviceRequestGroup.Count(sr => sr.Status == ServiceRequestStatus.Attending);
                int ClosedRequests = serviceRequestGroup.Count(sr => sr.Status == ServiceRequestStatus.Closed);
                string AverageCompletionTime = averageCompletionTime.ToString() + "hs";

                reportList.Add((maintenancePersonName, OpenRequests, AttendingRequests, ClosedRequests, AverageCompletionTime));

            }
            
            return reportList;
        }

        private double CalculateAverageCompletionTime(IEnumerable<ServiceRequest> serviceRequests)
        {
            double totalHours = 0;
            int totalRequests = 0;

            foreach (var request in serviceRequests)
            {
                if (request.Status == ServiceRequestStatus.Closed)
                {
                    TimeSpan difference = (TimeSpan)(request.EndDate - request.StartDate);
                    totalHours += difference.TotalHours;
                    totalRequests++;
                }
            }

            if (totalRequests == 0)
            {
                return 0;
            }

            return totalHours / totalRequests;
        }

        public IEnumerable<(string, int, int, int)> GetReport(Guid userId, string param)
        {
            List<Building> buildings = _buildingRepository.GetAllBuildingsByManagerId(userId);

            if (param != null)
            {
                bool HasBuilding = false;
                foreach (var building in buildings)
                {
                    if (building.Name == param)
                    {
                        HasBuilding = true;
                        buildings = new List<Building> { building };
                        break;
                    }
                }
                if (!HasBuilding) { 
                    throw new InvalidOperationException("Manager does not have any assigned buildings with name.");
                }
            }

            if (buildings == null || !buildings.Any())
            {
                throw new InvalidOperationException("Manager does not have any assigned buildings.");
            }

            List<(string, int, int, int)> reportList = new List<(string, int, int, int)>();

            foreach (var building in buildings)
            {

                IEnumerable<ServiceRequest> requests = _serviceRequestRepository.GetAllServiceRequests()
                .Where(req => req.Building.Id == building.Id);

                string buildingName = building.Name ?? "Unknown";
                int openCount = requests.Count(req => req.Status == ServiceRequestStatus.Open);
                int inProgressCount = requests.Count(req => req.Status == ServiceRequestStatus.Attending);
                int closedCount = requests.Count(req => req.Status == ServiceRequestStatus.Closed);

                reportList.Add((buildingName, openCount, inProgressCount, closedCount));
            }

            return reportList;
        }
    }
}
