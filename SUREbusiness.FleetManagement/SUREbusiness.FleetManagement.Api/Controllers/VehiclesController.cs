using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using SUREbusiness.FleetManagement.BLL.Commands;
using SUREbusiness.FleetManagement.BLL.Models;
using SUREbusiness.FleetManagement.BLL.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SUREbusiness.FleetManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IMediator mediator, ILogger<VehiclesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Pagination<Vehicle>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string loanedTo, string status, int page, int pageSize)
        {
            try
            {
                var vehiclesQuery = new GetVehiclesQuery(loanedTo, status, page, pageSize);
                var vehicles = await _mediator.Send(vehiclesQuery);

                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not get vehicles", ex);
                return Problem("Voertuigen konden niet worden opgehaald");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Vehicle), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var vehicle = await _mediator.Send(new GetVehicleByIdQuery(id));

                if (vehicle is null)
                {
                    return NotFound();
                }

                return Ok(vehicle);
            } catch(Exception ex)
            {
                _logger.LogError($"Could not get vehicle with id {id}", ex);
                return Problem("Voertuig kon niet opgehaald worden");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Vehicle), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateVehicleCommand createVehicleCommand)
        {
            try
            {
                var createVehicleResult = await _mediator.Send(createVehicleCommand);

                if (createVehicleResult.HasErrors)
                {
                    return BadRequest(createVehicleResult.Errors);
                }

                return CreatedAtAction(nameof(GetById), new { id = createVehicleResult.Result.Id }, createVehicleResult.Result);
            } catch(Exception ex)
            {
                _logger.LogError("Could not create vehicle", ex);
                return Problem("Voertuig kon niet toegevoegd worden");
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(Vehicle), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int id, [BindRequired] JsonPatchDocument<VehiclePatchModel> vehiclePatchModelDocument)
        {
            try
            {
                var vehicle = await _mediator.Send(new GetVehicleByIdQuery(id));

                if (vehicle is null)
                {
                    return NotFound();
                }

                var command = new PatchVehicleCommand(id, vehiclePatchModelDocument);
                var patchVehicleResult = await _mediator.Send(command);

                if (patchVehicleResult.HasErrors)
                {
                    return BadRequest(patchVehicleResult.Errors);
                }

                return Ok(patchVehicleResult.Result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not patch vehicle with id {id}", ex);
                return Problem("Voertuig kon niet worden bijgewerkt");
            }
        }
    }
}
