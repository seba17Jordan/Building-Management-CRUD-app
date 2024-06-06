using Domain;
using Domain.@enum;
using ModelsApi.Out;
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

        //Por mas que no use lo defino:
        public Guid? Id { get; set; }
        public bool? HasManager { get; set; }
        public string? ManagerName { get; set; }

        public Building ToEntity()
        {
            var building = new Building();
            var constructionCom = new ConstructionCompany(ConstructionCompany);
            building.Apartments = new List<Apartment>();

            if (Name != null) building.Name = Name;
            if (Address != null) building.Address = Address;
            if (ConstructionCompany != null) building.ConstructionCompany = constructionCom;
            if (CommonExpenses != null) building.CommonExpenses = CommonExpenses.Value;
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