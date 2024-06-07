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
        public Building CreateBuilding(Building building, User constructionComAdmin)
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

            ConstructionCompany existingCompany = _constructionCompanyRepository.GetConstructionCompanyByAdmin(constructionComAdmin.Id);

            if (existingCompany != null)
            {
                building.ConstructionCompany = existingCompany;
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

            if (building.Manager == null)
            {
                throw new InvalidOperationException("Building has no manager");
            }

            User currentManager = _userRepository.GetUserById(managerId);

            if (currentManager == null)
            {
                throw new ObjectNotFoundException("Manager not found");
            }

            if (building.Manager.Id != managerId)
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

        public Building UpdateBuildingById(Guid id, Building buildingUpdates, Guid comAdminId)
        {
            Building buildingToUpdate = _buildingRepository.GetBuildingById(id);

            if (buildingToUpdate == null)
            {
                throw new ArgumentNullException("Building not found");
            }

            if(buildingToUpdate.ConstructionCompanyAdmin.Id != comAdminId)
            {
                throw new ArgumentException("Construction Company Administrator is not the owner of the building");
            }

            if (buildingUpdates.Name != null)
            {
                if (_buildingRepository.BuildingNameExists(buildingUpdates.Name))
                {
                    throw new ArgumentException("Building with same name already exists");
                }
                buildingToUpdate.Name = buildingUpdates.Name;
            }

            if (buildingUpdates.Address != null) buildingToUpdate.Address = buildingUpdates.Address;
            if (buildingUpdates.ConstructionCompany != null) buildingToUpdate.ConstructionCompany = buildingUpdates.ConstructionCompany;
            if (buildingUpdates.CommonExpenses != null) buildingToUpdate.CommonExpenses = buildingUpdates.CommonExpenses;

            if (buildingUpdates.Apartments != null && buildingUpdates.Apartments.Count >=1) //se cambian todos los apartamentos de una
            {
                buildingToUpdate.Apartments.Clear();
                foreach (var apartment in buildingUpdates.Apartments)
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

            if(buildingToUpdate.Manager != null && newManagerId == buildingToUpdate.Manager.Id)
            {
                throw new InvalidOperationException("New manager is already the manager of the building");
            }

            if (buildingToUpdate.ConstructionCompanyAdmin.Id != constructionCompanyAdminId)
            {
                throw new InvalidOperationException("Construction company admin is not the owner of the building");
            }

            buildingToUpdate.Manager = newManager;
            _buildingRepository.UpdateBuilding(buildingToUpdate);
            _buildingRepository.Save();
            return buildingToUpdate;
        }

        public User GetManagerByName(string managerName)
        {
            return _userRepository.GetUserByName(managerName);
        }

        public IEnumerable<Building> GetBuildingsByManagerId(Guid managerId)
        {
            if (managerId == Guid.Empty)
            {
                throw new EmptyFieldException("Id is empty");
            }

            return _buildingRepository.GetAllBuildingsByManager(managerId);
        }
    }
}
