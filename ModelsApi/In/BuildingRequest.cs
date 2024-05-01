using Domain;
using System.Collections.Generic;

namespace ModelsApi.In
{
    public class BuildingRequest
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ConstructionCompany { get; set; }
        public int? CommonExpenses { get; set; }
        public List<ApartmentRequest>? Apartments { get; set; }

        public Guid managerId { get; set; } //sacar esto, obtenerlo del token

        public Building ToEntity()
        {
            var building = new Building();
            building.Apartments = new List<Apartment>();

            if (Name != null)
            {
                building.Name = Name;
            }

            if (Address != null)
            {
                building.Address = Address;
            }

            if (ConstructionCompany != null)
            {
                building.ConstructionCompany = ConstructionCompany;
            }

            if (CommonExpenses != null)
            {
                building.CommonExpenses = CommonExpenses.Value;
            }
        
            if (Apartments != null)
            {
                foreach (var apartmentReq in Apartments)
                {
                    building.Apartments.Add(apartmentReq.ToEntity());
                }
            }
            if (managerId != null) { 
                building.managerId = managerId;
            }

            return building;
        }
    }
}