using Domain;

namespace ModelsApi
{
    public class ApartmentRequest
    {
        public int Floor { get; set; }
        public int Number { get; set; }
        public OwnerRequest Owner { get; set; }
        public int Rooms { get; set; }
        public int Bathrooms { get; set; }
        public bool HasTerrace { get; set; }

        public Apartment ToEntity()
        {
            return new Apartment 
            { Floor = Floor,
                Number = Number,
                Owner = Owner.ToEntity(),
                Rooms = Rooms,
                Bathrooms = Bathrooms,
                HasTerrace = HasTerrace 
            };
        }
    }
}