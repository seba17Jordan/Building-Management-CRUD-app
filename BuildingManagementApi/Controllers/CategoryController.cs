﻿using Microsoft.AspNetCore.Mvc;
using LogicInterface;
using ModelsApi.In;
using ModelsApi.Out;

namespace BuildingManagementApi.Controllers
{
    [ApiController]
    [Route("Api/Categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryLogic _categoryLogic;
        public CategoryController(ICategoryLogic categoryLogic)
        {
            this._categoryLogic = categoryLogic;
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryRequest categoryToCreate)
        {
            var category = categoryToCreate.ToEntity();
            var resultObj = _categoryLogic.CreateCategory(category);
            var outputResult = new CategoryResponse(resultObj);

            return CreatedAtAction(nameof(CreateCategory), new { id = outputResult.Id }, outputResult);
        }
    }
}
