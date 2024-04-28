namespace Domain
{
    public class Apartment
    {
        public Guid Id { get; set; }
        public int Floor { get; set; }
        public int Number { get; set; }
        public Owner Owner { get; set; }
        public int Rooms { get; set; }
        public int Bathrooms { get; set; }
        public bool HasTerrace { get; set; }

        public Apartment() { }
    }
}