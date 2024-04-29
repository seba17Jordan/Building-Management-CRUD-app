namespace Domain
{
    public class Apartment
    {
        public Guid Id { get; set; }
        public int Floor { get; set; }
        public int Number { get; set; }
        public Owner? Owner { get; set; }
        public int Rooms { get; set; }
        public int Bathrooms { get; set; }
        public bool HasTerrace { get; set; }

        public Apartment() { }

        public override bool Equals(object apartment)
        {
            if (apartment == null || GetType() != apartment.GetType())
            {
                return false;
            }

            Apartment secondApartment = (Apartment)apartment;

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