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
        private readonly ISessionService _sessionService;
        public ConstructionCompanyController(IConstructionCompanyLogic constructionLogic,ISessionService sessionService)
        {
            _constructionCompanyLogic = constructionLogic;
            _sessionService = sessionService;
        }

        [HttpPost]
        //[ServiceFilter(typeof(AuthenticationFilter))]
        //[AuthorizationFilter(_currentRole = Roles.ConstructionCompanyAdmin)]
        public IActionResult CreateConstructionCompany([FromBody] ConstructionCompanyRequest constructionCompanyToCreate)
        {
            string token = Request.Headers["Authorization"].ToString();
            var constructionCompanyAdmin = _sessionService.GetUserByToken(Guid.Parse(token));

            var ConstructionCompany = constructionCompanyToCreate.ToEntity();
            ConstructionCompany.ConstructionCompanyAdmin = constructionCompanyAdmin;
            var resultObj = _constructionCompanyLogic.CreateConstructionCompany(ConstructionCompany);
            var outputResult = new ConstructionCompanyResponse(resultObj);

            return CreatedAtAction(nameof(CreateConstructionCompany), new { id = outputResult.Id }, outputResult);
        }
    }
}
