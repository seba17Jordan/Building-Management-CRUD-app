using Domain;
using Domain.@enum;

namespace ModelsApi.In
{
    public class MaintenancePersonRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public Roles Role { get; set; }

        public User ToEntity()
        {
            var maintenancePerson = new User
            {
                Email = Email,
                Password = Password,
                Name = Name,
                LastName = LastName,
                Role = Roles.Maintenance
            };

            return maintenancePerson;
        }
    }
}