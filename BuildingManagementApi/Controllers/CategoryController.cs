using Microsoft.AspNetCore.Mvc;
using LogicInterface;
using ModelsApi.In;
using ModelsApi.Out;
using BuildingManagementApi.Filters;
using Domain.@enum;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [TypeFilter(typeof(ExceptionFilter))]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryLogic _categoryLogic;
        public CategoryController(ICategoryLogic categoryLogic)
        {
            this._categoryLogic = categoryLogic;
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthenticationFilter))]
        [AuthorizationFilter(_currentRole = Roles.Administrator)]
        public IActionResult CreateCategory([FromBody] CategoryRequest categoryToCreate)
        {
            var category = categoryToCreate.ToEntity();
            var resultObj = _categoryLogic.CreateCategory(category);
            var outputResult = new CategoryResponse(resultObj);

            return CreatedAtAction(nameof(CreateCategory), new { id = outputResult.Id }, outputResult);
        }
    }
}
