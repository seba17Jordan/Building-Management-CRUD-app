using Domain;

namespace ModelsApi
{
    public class OwnerRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Owner ToEntity()
        {
            return new Owner
            { 
                Name = Name,
                LastName = LastName,
                Email = Email
            };
        }
    }
}