using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDb_CRUD.Services.DTO;
using MongoDb_CRUD.Services.Interfaces;

namespace MongoDb_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(IAppointment _appointment) : ControllerBase
    {
        [HttpGet("All-Appointments")]
        public async Task<IActionResult> AllAppointments()
        {
            var appointments = await _appointment.fetchAppointments();
            return Ok(appointments);
        }


        [HttpGet("Fetch-Appointment")]
        public async Task<IActionResult> FetchAppointment(string id)
        {
            var appointments = await _appointment.fetchAppointment(id);
            return Ok(appointments);
        }

        [HttpPost("Create-New-Appointment")]
        public async Task<IActionResult> CreateNewAppointment(CreateAppointmentDTO createAppointment)
        {
            var (isSuccess, message) = await _appointment.AddNewAppointment(createAppointment);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpPut("Update-Appointment")]
        public async Task<IActionResult> UpdateAppointment(string id, UpdateAppointmentDTO updateAppointment)
        {
            var (isSuccess, message) = await _appointment.UpdateAppointment(id, updateAppointment);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpDelete("Delete-Appointment")]
        public async Task<IActionResult> DeleteAppointment(string id)
        {
            var (isSuccess, message) = await _appointment.DeleteAppointment(id);

            if (!isSuccess)
                return BadRequest(message);

            return Ok(message);
        }


    }
}
