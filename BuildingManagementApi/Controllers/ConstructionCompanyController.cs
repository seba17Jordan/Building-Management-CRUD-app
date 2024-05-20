using Microsoft.AspNetCore.Mvc;
using LogicInterface;
using ModelsApi.In;
using ModelsApi.Out;
using BuildingManagementApi.Filters;
using Domain.@enum;
using CustomExceptions;
using Domain;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("api/constructionCompanies")]
    [ExceptionFilter]
    public class ConstructionCompanyController : ControllerBase
    {
        private readonly IConstructionCompanyLogic _constructionCompanyLogic;
        public ConstructionCompanyController(IConstructionCompanyLogic constructionLogic)
        {
            this._constructionCompanyLogic = constructionLogic;
        }

        [HttpPost]
        //[ServiceFilter(typeof(AuthenticationFilter))]
        //[AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult CreateConstructionCompany([FromBody] ConstructionCompanyRequest constructionCompanyToCreate)
        {
            throw new NotImplementedException();
        }
    }
}
