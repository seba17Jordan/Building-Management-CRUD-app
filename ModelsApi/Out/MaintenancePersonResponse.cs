using Domain;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class MaintenancePersonResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public MaintenancePersonResponse(User maintenancePerson)
        {
            Id = maintenancePerson.Id;
            Name = maintenancePerson.Name;
            LastName = maintenancePerson.LastName;
            Email = maintenancePerson.Email;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            MaintenancePersonResponse other = (MaintenancePersonResponse)obj;
            return Id == other.Id && Name == other.Name && LastName == other.LastName && Email == other.Email;
        }
    }
}
