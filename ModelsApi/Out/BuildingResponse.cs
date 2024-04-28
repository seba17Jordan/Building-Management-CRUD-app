using Domain;

namespace ModelsApi.Out
{
    public class BuildingResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ConstructionCompany { get; set; }
        public int CommonExpenses { get; set; }
        public List<ApartmentResponse> Apartments { get; set; }

        public BuildingResponse(Building expectedBuilding)
        {
            Id = expectedBuilding.Id;
            Name = expectedBuilding.Name;
            Address = expectedBuilding.Address;
            ConstructionCompany = expectedBuilding.ConstructionCompany;
            CommonExpenses = (int)expectedBuilding.CommonExpenses;
            Apartments = new List<ApartmentResponse>();

            if (expectedBuilding.Apartments != null)
            {
                foreach (var apartment in expectedBuilding.Apartments)
                {
                    Apartments.Add(new ApartmentResponse(apartment));
                }
            }
            
        }

        public override bool Equals(object building)
        {
            if (building == null || GetType() != building.GetType())
            {
                return false;
            }
            BuildingResponse secondBuilding = (BuildingResponse)building;

            return Id == secondBuilding.Id &&
                   Name == secondBuilding.Name &&
                   Address == secondBuilding.Address &&
                   ConstructionCompany == secondBuilding.ConstructionCompany &&
                   CommonExpenses == secondBuilding.CommonExpenses &&
                   Apartments.SequenceEqual(secondBuilding.Apartments);
        }

    }
}