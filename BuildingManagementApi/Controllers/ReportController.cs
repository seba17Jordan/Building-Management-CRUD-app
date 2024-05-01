using BuildingManagementApi.Filters;
using BusinessLogic;
using DataAccess;
using Domain.@enum;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsApi;
using ModelsApi.In;
using ModelsApi.Out;
using System.Globalization;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ReportController : ControllerBase
    {
        private readonly IReportLogic _reportLogic;
        private readonly ISessionService _sessionService;

        public ReportController(IReportLogic reportLogic, ISessionService sessionService)
        {
            _reportLogic = reportLogic;
            _sessionService = sessionService;
        }

        [HttpGet]
        //[ServiceFilter(typeof(AuthenticationFilter))]
        //[AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult GetReport([FromQuery] string? building)
        {
            string token = Request.Headers["Authorization"].ToString();
            var user = _sessionService.GetUserByToken(Guid.Parse(token));
            var reportInfo = _reportLogic.GetReport(user.Id, building);
            var response = reportInfo.Select(t => new ReportResponse(t));
            return Ok(response);
        }

        [HttpGet("{buildingName}")]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Manager)]
        public IActionResult GetMaintenanceReport([FromRoute] string buildingName, [FromQuery] string? maintenanceName)
        {
            string token = Request.Headers["Authorization"].ToString();
            var manager = _sessionService.GetUserByToken(Guid.Parse(token));

            var reportInfo = _reportLogic.GetMaintenanceReport(buildingName, manager.Id, maintenanceName);
            var response = reportInfo.Select(t => new MaintenanceReportResponse(t));
            return Ok(response);
        }
    }
}
