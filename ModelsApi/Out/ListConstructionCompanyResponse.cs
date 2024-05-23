using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class ListConstructionCompanyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool HasManager { get; set; }
        public string ManagerName { get; set; }

        public ListConstructionCompanyResponse(Guid id, string name, string address, bool hasManager, string managerName)
        {
            Id = id;
            Name = name;
            Address = address;
            HasManager = hasManager;
            ManagerName = managerName;
        }

        public override bool Equals(object obj)
        {
            return obj is ListConstructionCompanyResponse response &&
                   Id.Equals(response.Id) &&
                   Name == response.Name &&
                   Address == response.Address &&
                   HasManager == response.HasManager &&
                   ManagerName == response.ManagerName;
        }
    }

}
