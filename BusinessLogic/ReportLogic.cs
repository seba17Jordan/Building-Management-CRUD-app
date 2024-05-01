using Domain.@enum;
using Domain;
using IDataAccess;
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

        public IEnumerable<(string, int, int, int)> GetReport(Guid userId, string param)
        {
            List<Building> buildings = _buildingRepository.GetAllBuildings(userId);

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
                // Obtener las solicitudes filtradas por el edificio actual
                IEnumerable<ServiceRequest> requests = _serviceRequestRepository.GetAllServiceRequests()
                    .Where(req => req.BuildingId == building.Id);

                // Agrupar las solicitudes por BuildingId
                //var requestsByBuilding = requests.GroupBy(req => req.BuildingId);

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
