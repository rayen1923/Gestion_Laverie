using GestionLaverie.Entites;
using LaverieController.Modele.Business;
using Microsoft.AspNetCore.Mvc;

namespace LaverieController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly MachineBusiness _business;

        public MachineController(MachineBusiness business)
        {
            _business = business;
        }

        [HttpPut("toggleMachineEtat/{machineId}/{cycleId}")]
        public IActionResult ToggleMachineEtat(int machineId)
        {
            try
            {
                bool isUpdated = _business.ToggleMachineEtat(machineId);

                if (isUpdated)
                {
                    return Ok($"Machine with ID {machineId} state successfully toggled.");
                }
                else
                {
                    return NotFound($"Machine with ID {machineId} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
