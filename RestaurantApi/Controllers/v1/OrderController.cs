using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Core.Application.Interfaces.Services;
using RestaurantApi.Core.Application.ViewModels.Orders;

namespace RestaurantApi.Presentation.WebApi.Controllers.v1
{
    [Authorize(Roles = "Mesero")]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }
        [ApiVersion("1.0")]

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(ListOrderViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                await _service.AddService(vm, vm.DishesIds);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                var orders = await _service.GetAllWithInclude();

                if (orders == null || orders.Count == 0) return NotFound();

                return Ok(orders);
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
                var order = await _service.GetByIdWithInclude(id);

                if (order == null) return NotFound();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ApiVersion("1.0")]

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, UpdateOrderViewModel vm)
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
                return BadRequest(ex.Message);
            }


        }

    }
}
