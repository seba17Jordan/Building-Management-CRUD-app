
using CustomExceptions;

namespace Domain
{
    public class Building
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public ConstructionCompany? ConstructionCompany { get; set; }
        public int? CommonExpenses { get; set; }
        public List<Apartment>? Apartments { get; set; }

        public Guid managerId { get; set; }

        public Building() { }

        public void SelfValidate()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Address))
            {
                throw new EmptyFieldException("There are empty fields");
            }

            if (CommonExpenses < 0)
            {
                throw new ArgumentException("Common expenses must be greater than 0");
            }

            if (Apartments == null || Apartments.Count == 0)
            {
                throw new ArgumentException("Building must have at least one apartment");
            }

            ConstructionCompany.SelfValidate();
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
                   ConstructionCompany.Equals(secondBuilding.ConstructionCompany) &&
                   CommonExpenses == secondBuilding.CommonExpenses &&
                   Apartments.SequenceEqual(secondBuilding.Apartments);
        }
    }
}