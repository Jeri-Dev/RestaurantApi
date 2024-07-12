using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Core.Application.Interfaces.Services;
using RestaurantApi.Core.Application.ViewModels.Orders;
using RestaurantApi.Core.Application.ViewModels.Table;

namespace RestaurantApi.Presentation.WebApi.Controllers.v1
{
    public class TableController : BaseApiController
    {
        private readonly ITableService _service;
        public TableController(ITableService service)
        {
            _service = service;
        }
        [ApiVersion("1.0")]
        [Authorize(Roles = "Administrador")]

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(TableViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest();
            vm.State = "Disponible";
            await _service.AddService(vm);
            return Ok();
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
                var tables = await _service.GetAllServiceList();

                if (tables == null || tables.Count == 0) return NotFound();

                return Ok(tables);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [ApiVersion("1.0")]
        [Authorize(Roles = "Administrador")]

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, UpdateTableViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                var table = await _service.GetByIdService(id);
                vm.Id = table.Id;

                await _service.UpdateService(vm, id);
                return Ok(await _service.GetByIdService(id));
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
                var table = await _service.GetByIdService(id);

                if (table == null) return NotFound();

                return Ok(table);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [ApiVersion("1.0")]
        [HttpGet("{id}/orders")]
        [Authorize(Roles = "Mesero")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTableOrden(int id)
        {
            try
            {
                List<OrderViewModel> tableOrders = await _service.GetTableOrden(id);

                return Ok(tableOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [ApiVersion("1.0")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeStatus(int id, ChangeStatusTableViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                var table = await _service.GetByIdService(id);
                vm.Id = table.Id;
                vm.Description = table.Description;
                vm.Capacity = table.Capacity;

                await _service.ChangeStatusService(vm, id);
                return Ok(await _service.GetByIdService(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
