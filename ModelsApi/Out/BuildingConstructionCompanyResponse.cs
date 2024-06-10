using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class BuildingConstructionCompanyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool HasManager { get; set; }
        public string ManagerName { get; set; }

        public BuildingConstructionCompanyResponse(Building b)
        {
            Id = b.Id;
            Name = b.Name;
            Address = b.Address;
            HasManager = b.Manager != null;
        }

        public override bool Equals(object obj)
        {
            return obj is BuildingConstructionCompanyResponse response &&
                   Id.Equals(response.Id) &&
                   Name == response.Name &&
                   Address == response.Address &&
                   HasManager == response.HasManager &&
                   ManagerName == response.ManagerName;
        }
    }

}
