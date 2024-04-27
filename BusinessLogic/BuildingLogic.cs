﻿using Domain;
using IDataAccess;
using LogicInterface;
using System.Globalization;

namespace BusinessLogic
{
    public class BuildingLogic : IBuildingLogic
    {
        private readonly IBuildingRepository _buildingRepository;
        public BuildingLogic(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }
        public Building CreateBuilding(Building building)
        {
            if (building == null)
            {
                throw new ArgumentNullException(nameof(building), "Building is null");
            }
            if (string.IsNullOrWhiteSpace(building.Name))
            {
                throw new ArgumentException("Building invalid name", nameof(building.Name));
            }
            if (string.IsNullOrWhiteSpace(building.Address))
            {
                throw new ArgumentException("Building invalid address", nameof(building.Address));
            }
            if (string.IsNullOrWhiteSpace(building.ConstructionCompany))
            {
                throw new ArgumentException("Building invalid construction company", nameof(building.ConstructionCompany));
            }
            return _buildingRepository.CreateBuilding(building);
        }
    }
}
