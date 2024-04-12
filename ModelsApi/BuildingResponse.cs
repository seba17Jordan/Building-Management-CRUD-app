using Domain;

namespace ModelsApi
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
            this.Id = expectedBuilding.Id;
            this.Name = expectedBuilding.Name;
            this.Address = expectedBuilding.Address;
            this.ConstructionCompany = expectedBuilding.ConstructionCompany;
            this.CommonExpenses = expectedBuilding.CommonExpenses;
            this.Apartments = new List<ApartmentResponse>();
            
            foreach (var apartment in expectedBuilding.Apartments)
            {
                this.Apartments.Add(new ApartmentResponse(apartment));
            }
        }
    }
}