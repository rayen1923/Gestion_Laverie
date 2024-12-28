using GestionLaverie.Entites;
using LaverieController.Modele.Business;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LaverieController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationBusiness _business;

        public ConfigurationController(ConfigurationBusiness business)
        {
            _business = business;
        }

        [HttpGet("proprietaires")]
        public ActionResult<List<Propriétaire>> GetPropriétairesWithDetails()
        {
            try
            {
                var result = _business.GetAllPropriétairesWithDetails();
                if (result.Count == 0)
                {
                    return NotFound("No proprietors found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("login")]
        public IActionResult Login([FromQuery] string username, [FromQuery] string password)
        {
            try
            {
                int userId = _business.Login(username, password);
                if (userId > 0)
                {
                    return Ok(new { Message = "Login successful", UserId = userId });
                }
                else
                {
                    return Unauthorized(new { Message = "Invalid credentials" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
