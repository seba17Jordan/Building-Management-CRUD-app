using ImportersInterface;
using LogicInterface;
using ModelsApi.In;
using Newtonsoft.Json;

namespace JsonImporter
{
    public class JsonBuildingImporter : IBuildingImporter
    {
        private readonly IBuildingService _buildingService;
        public JsonBuildingImporter(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        public void Import(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            var buildingsToImport = JsonConvert.DeserializeObject<List<BuildingRequest>>(jsonContent);

            foreach (var buildingRequest in buildingsToImport)
            {
                _buildingService.CreateBuilding(buildingRequest);
            }

            Console.WriteLine("Importando desde JSON");
        }
    }
}
