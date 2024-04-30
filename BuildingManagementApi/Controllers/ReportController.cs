using BuildingManagementApi.Filters;
using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using ModelsApi;
using ModelsApi.In;
using ModelsApi.Out;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ReportController : ControllerBase
    {
        private readonly IReportLogic _reportLogic;

        public ReportController(IReportLogic reportLogic)
        {
            _reportLogic = reportLogic;
        }

        [HttpGet]
        //[AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult GetReport([FromQuery] string? param)
        {
            //var userId = Guid.Parse(HttpContext.Items["UserId"] as string);
            // Hardcodear un valor Guid
            Guid userId = new Guid("38352B3A-31A1-43E6-8A81-76B97A8F3A1A");
            var reportInfo = _reportLogic.GetReport(userId, param);
            var response = reportInfo.Select(t => new ReportResponse(t));
            return Ok(response);
        }
    }
}
