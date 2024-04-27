using Domain;

namespace ModelsApi.In
{
    public class MaintenancePersonRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public User ToEntity()
        {
            var maintenancePerson = new User
            {
                Email = Email,
                Password = Password,
                Name = Name,
                LastName = LastName
            };

            return maintenancePerson;
        }
    }
}