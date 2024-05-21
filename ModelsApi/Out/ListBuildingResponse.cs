using System;

namespace ModelsApi.Out
{
    public class ListBuildingResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool HasManager { get; set; }
        public string? ManagerName { get; set; }

        public ListBuildingResponse(Guid id, string name, string address, bool hasManager, string? managerName)
        {
            Id = id;
            Name = name;
            Address = address;
            HasManager = hasManager;
            ManagerName = managerName;
        }
    }
}
