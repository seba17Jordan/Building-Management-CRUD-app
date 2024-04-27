using Domain;

namespace ModelsApi.Out
{
    public class ApartmentResponse
    {
        public Guid Id { get; set; }
        public int Floor { get; set; }
        public int Number { get; set; }
        public OwnerResponse Owner { get; set; }
        public int Rooms { get; set; }
        public int Bathrooms { get; set; }
        public bool HasTerrace { get; set; }

        public ApartmentResponse(Apartment apartment)
        {
            Floor = apartment.Floor;
            Number = apartment.Number;
            Owner = new OwnerResponse(apartment.Owner);
            Rooms = apartment.Rooms;
            Bathrooms = apartment.Bathrooms;
            HasTerrace = apartment.HasTerrace;
        }

        public override bool Equals(object apartment)
        {
            if (apartment == null || GetType() != apartment.GetType())
            {
                return false;
            }

            ApartmentResponse secondApartment = (ApartmentResponse)apartment;

            // Comparación de propiedades
            return Floor == secondApartment.Floor &&
                   Number == secondApartment.Number &&
                   Owner.Equals(secondApartment.Owner) &&
                   Rooms == secondApartment.Rooms &&
                   Bathrooms == secondApartment.Bathrooms &&
                   HasTerrace == secondApartment.HasTerrace;
        }
    }
}