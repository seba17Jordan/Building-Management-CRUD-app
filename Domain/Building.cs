
namespace Domain
{
    public class Building
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ConstructionCompany { get; set; }
        public int CommonExpenses { get; set; }
        public List<Apartment> Apartments { get; set; }

        public Building()
        {
        }
    }
}