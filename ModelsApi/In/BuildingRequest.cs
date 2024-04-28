using Domain;

namespace ModelsApi.In
{
    public class BuildingRequest
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ConstructionCompany { get; set; }
        public int? CommonExpenses { get; set; }
        public List<ApartmentRequest>? Apartments { get; set; }

        public Building ToEntity()
        {
            var building = new Building
            {
                Name = Name,
                Address = Address,
                ConstructionCompany = ConstructionCompany,
                CommonExpenses = (int)CommonExpenses,
                Apartments = new List<Apartment>()
            };

            if (Apartments != null)
            {
                foreach (var apartmentReq in Apartments)
                {
                    building.Apartments.Add(apartmentReq.ToEntity());
                }
            }

            return building;
        }
    }
}