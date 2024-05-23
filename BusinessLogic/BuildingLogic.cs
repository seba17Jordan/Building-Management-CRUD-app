using CustomExceptions;
using Domain;
using IDataAccess;
using LogicInterface;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace BusinessLogic
{
    public class BuildingLogic : IBuildingLogic
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IConstructionCompanyRepository _constructionCompanyRepository;
        private readonly IUserRepository _userRepository;
        public BuildingLogic(IBuildingRepository buildingRepository, IServiceRequestRepository serviceRequestRepository, IConstructionCompanyRepository constructionCompanyRepository, IUserRepository userRepository)
        {
            _buildingRepository = buildingRepository;
            _serviceRequestRepository = serviceRequestRepository;
            _constructionCompanyRepository = constructionCompanyRepository;
            _userRepository = userRepository;
        }
        public Building CreateBuilding(Building building)
        {
            if (building == null)
            {
                throw new ArgumentNullException(nameof(building), "Building is null");
            }

            building.SelfValidate();

            foreach (var apartment in building.Apartments)
            {
                apartment.SelfValidate();
                apartment.Owner.SelfValidate();
            }
            
            if (_buildingRepository.BuildingNameExists(building.Name))
            {
                throw new ObjectAlreadyExistsException("Building with same name already exists");
            }

            ConstructionCompany existingCompany = _constructionCompanyRepository.GetConstructionCompanyByName(building.ConstructionCompany.Name);
            
            if (existingCompany != null)
            {
                if (existingCompany.ConstructionCompanyAdmin.Equals(building.ConstructionCompanyAdmin))
                {
                    building.ConstructionCompany = existingCompany;
                }
                else
                {
                    throw new InvalidOperationException("This construction company does not belong to this construction company administrator");
                }
            }
            else
            {
                throw new ObjectNotFoundException("Construction company not found");
            }

            return _buildingRepository.CreateBuilding(building);
        }

        public void DeleteBuildingById(Guid id, Guid managerId)
        {
            if (id == Guid.Empty)
            {
                throw new EmptyFieldException("Id is empty");
            }

            Building building = _buildingRepository.GetBuildingById(id);
            
            if (building == null)
            {
                throw new ObjectNotFoundException("Building not found");
            }
            
            if (building.managerId != managerId)
            {
                throw new InvalidOperationException("Manager is not the owner of the building");
            }

            //Si hay solicitud no cerrada asociada al edificio, no puedo borrarlo
            if (_serviceRequestRepository.GetNoClosedServiceRequestsByBuildingId(id).Count() > 0)
            {
                throw new InvalidOperationException("There are active service requests associated with this building");
            }
            
            if (building.Apartments != null)
            {
                foreach (var apartment in building.Apartments)
                {
                    if (apartment.Owner != null)
                    {
                        apartment.Owner = null;
                    }
                    _buildingRepository.DeleteApartment(apartment);
                }
                building.Apartments.Clear();
                building.Apartments = null;
            }
            _buildingRepository.Save();
            _buildingRepository.DeleteBuilding(building);
            _buildingRepository.Save();
        }

        public Building UpdateBuildingById(Guid id, Building building, Guid managerId)
        {
            Building buildingToUpdate = _buildingRepository.GetBuildingById(id);

            if (buildingToUpdate == null)
            {
                throw new ArgumentNullException("Building not found", nameof(id));
            }

            if(buildingToUpdate.managerId != managerId)
            {
                throw new ArgumentException("Manager is not the owner of the building");
            }

            if (building.Name != null)
            {
                if (_buildingRepository.BuildingNameExists(building.Name))
                {
                    throw new ArgumentException("Building with same name already exists");
                }
                buildingToUpdate.Name = building.Name;
            }

            if (building.Address != null)
            {
                buildingToUpdate.Address = building.Address;
            }

            if (building.ConstructionCompany != null)
            {
                buildingToUpdate.ConstructionCompany = building.ConstructionCompany;
            }

            if (building.CommonExpenses != null)
            {
                buildingToUpdate.CommonExpenses = building.CommonExpenses;
            }

            if (building.Apartments != null && building.Apartments.Count >=1) //se cambian todos los apartamentos de una
            {
                buildingToUpdate.Apartments.Clear();
                foreach (var apartment in building.Apartments)
                {
                    apartment.SelfValidate();
                    var clonedApartment = new Apartment
                    {
                        Number = apartment.Number,
                        Floor = apartment.Floor,
                        Owner = apartment.Owner,
                        Rooms = apartment.Rooms,
                        Bathrooms = apartment.Bathrooms,
                        HasTerrace = apartment.HasTerrace,
                    };
                    buildingToUpdate.Apartments.Add(clonedApartment);
                }
            }

            _buildingRepository.UpdateBuilding(buildingToUpdate);
            _buildingRepository.Save();

            return buildingToUpdate;
        }

        public string GetBuildingManagerName(Guid buildingId)
        {
            Building building = _buildingRepository.GetBuildingById(buildingId);
            if (building == null)
            {
                throw new ArgumentNullException("Building not found");
            }

            User manager = _userRepository.GetUserById(building.managerId);
            if (manager == null)
            {
                throw new ArgumentNullException("Manager not found");
            }

            return manager.Name;
        }

        public IEnumerable<Building> GetBuildingsByCompanyAdminId(Guid companyAdminId)
        {
            if (companyAdminId == Guid.Empty)
            {
                throw new EmptyFieldException("Id is empty");
            }

            return _buildingRepository.GetAllBuildings(companyAdminId);

        }

        public Building ModifyBuildingManager(Guid buildingId, Guid newManagerId, Guid constructionCompanyAdminId)
        {
            Building buildingToUpdate = _buildingRepository.GetBuildingById(buildingId);
            User newManager = _userRepository.GetUserById(newManagerId);

            if (buildingToUpdate == null)
            {
                throw new ArgumentNullException("Building not found");
            }

            if (newManager == null)
            {
                throw new ArgumentNullException("New manager not found");
            }

            if(newManagerId == buildingToUpdate.managerId)
            {
                throw new InvalidOperationException("New manager is already the manager of the building");
            }

            if (buildingToUpdate.ConstructionCompanyAdmin.Id != constructionCompanyAdminId)
            {
                throw new InvalidOperationException("Construction company admin is not the owner of the building");
            }

            buildingToUpdate.managerId = newManagerId;
            _buildingRepository.UpdateBuilding(buildingToUpdate);
            _buildingRepository.Save();
            return buildingToUpdate;
        }
    }
}
