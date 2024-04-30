
namespace Domain
{
    public class Building
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ConstructionCompany { get; set; }
        public int? CommonExpenses { get; set; }
        public List<Apartment>? Apartments { get; set; }

        public Guid managerId { get; set; }

        public Building()
        {
        }

        public override bool Equals(object building)
        {
            if (building == null || GetType() != building.GetType())
            {
                return false;
            }
            Building secondBuilding = (Building)building;

            return Name == secondBuilding.Name &&
                   Address == secondBuilding.Address &&
                   ConstructionCompany == secondBuilding.ConstructionCompany &&
                   CommonExpenses == secondBuilding.CommonExpenses &&
                   Apartments.SequenceEqual(secondBuilding.Apartments);
        }
    }
}