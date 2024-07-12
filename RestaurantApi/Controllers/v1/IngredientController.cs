using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Core.Application.Interfaces.Services;
using RestaurantApi.Core.Application.ViewModels.Ingredients;

namespace RestaurantApi.Presentation.WebApi.Controllers.v1
{
    [Authorize(Roles = "Administrador")]

    public class IngredientController : BaseApiController
    {
        private readonly IIngredientService _service;
        public IngredientController(IIngredientService service) {
            _service = service;
        }
        [ApiVersion("1.0")]

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List()
        {
            try
            {
                var ingredients = await _service.GetAllService();

                if (ingredients == null || ingredients.Count == 0) return NotFound();

                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ApiVersion("1.0")]

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ingredient = await _service.GetByIdService(id);

                if (ingredient == null) return NotFound();

                return Ok(ingredient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ApiVersion("1.0")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(IngredientViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest();
            if(vm.Name == "string") return BadRequest();

            await _service.AddService(vm);
            return Ok();
        }

        [ApiVersion("1.0")]

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, UpdateIngredientViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                var ingredient = await _service.GetByIdService(id);
                ingredient.Name = vm.Name;

                await _service.UpdateService(ingredient, id);
                return Ok(await _service.GetByIdService(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
