using ImportersInterface;
using LogicInterface;
using ModelsApi.In;
using Newtonsoft.Json;

namespace JsonImporter
{
    public class JsonBuildingImporter : IBuildingImporter
    {
        public JsonBuildingImporter() {}

        public void Import(string filePath, IBuildingService buildingService, string constructionCompanyAdminToken)
        {
            string jsonContent = File.ReadAllText(filePath);
            var buildingsToImport = JsonConvert.DeserializeObject<List<BuildingRequest>>(jsonContent);

            foreach (var buildingRequest in buildingsToImport)
            {
                buildingService.CreateBuilding(buildingRequest, constructionCompanyAdminToken);
            }

            Console.WriteLine("Importando desde JSON");
        }
    }
}
