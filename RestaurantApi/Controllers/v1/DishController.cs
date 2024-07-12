using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Core.Application.Interfaces.Services;
using RestaurantApi.Core.Application.ViewModels.Dish;


namespace RestaurantApi.Presentation.WebApi.Controllers.v1
{
    [Authorize(Roles = "Administrador")]
    
    public class DishController : BaseApiController
    {

        private readonly IDishService _service;
        public DishController(IDishService service)
        {
            _service = service;
        }

        [ApiVersion("1.0")]

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(ListDishViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (vm.Name == "string") return BadRequest("Debe de colocar un nombre");
                if (vm.Category == "string") return BadRequest("Debe de colocar una categoria");

                await _service.AddService(vm, vm.IngredientsIds);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
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
                var dishes = await _service.GetAllWithInclude();

                if (dishes == null || dishes.Count == 0) return NotFound();

                return Ok(dishes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

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
                var dish = await _service.GetByIdWithInclude(id);

                if (dish == null) return NotFound();

                return Ok(dish);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [ApiVersion("1.0")]

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, UpdateDishViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                var dish = await _service.GetByIdWithInclude(id);

                vm.Id = dish.Id;

                await _service.UpdateService(vm, id);
                return Ok(await _service.GetByIdWithInclude(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }


        }
    }
}
