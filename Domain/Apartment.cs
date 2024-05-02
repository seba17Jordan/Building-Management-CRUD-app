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

        public void SelfValidate()
        {
            if (Floor < 0 || Number < 0 || Rooms < 0 || Bathrooms < 0)
            {
                throw new ArgumentException("All apartment fields must be grater than zero");
            }

            if (Owner == null)
            {
                throw new ArgumentNullException("Owner must be set in all apartments");
            }
        }

        public override bool Equals(object apartment)
        {
            if (apartment == null || GetType() != apartment.GetType())
            {
                return false;
            }

            Apartment secondApartment = (Apartment)apartment;

            return Floor == secondApartment.Floor &&
                   Number == secondApartment.Number &&
                   Owner.Equals(secondApartment.Owner) &&
                   Rooms == secondApartment.Rooms &&
                   Bathrooms == secondApartment.Bathrooms &&
                   HasTerrace == secondApartment.HasTerrace;
        }

        
    }
}