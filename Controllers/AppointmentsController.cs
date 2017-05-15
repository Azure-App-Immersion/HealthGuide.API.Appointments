using HealthGuide.API.Appointments.Data;
using HealthGuide.API.Appointments.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthGuide.API.Appointments.Controllers
{
    [Route("[controller]")]
    [ResponseCache(Duration = 30)]
    public class AppointmentsController : Controller
    {
        private AppointmentsContext _appointmentsContext;

        public AppointmentsController(AppointmentsContext appointmentsContext)
        {
            _appointmentsContext = appointmentsContext;
        }

        [HttpGet("{id}", Name = "GetAppointment")]
        public async Task<IActionResult> Get(string id)
        {
            var appointment = await _appointmentsContext.GetAppointmentAsync(id);
            if (appointment == null) {
                return NotFound();
            }
            return Ok(appointment);
        }

        [HttpGet("date/{date}", Name = "GetAppointmentsForDate")]
        public async Task<IEnumerable<Appointment>> Get(DateTimeOffset date)
        {
            return await _appointmentsContext.GetAppointmentsForDateAsync(date);
        }

        [HttpGet("name/{firstName}/{lastName}", Name = "GetAppointmentForFirstNameLastName")]
        public async Task<IActionResult> Get(string firstName, string lastName)
        {
            var appointment = await _appointmentsContext.GetAppointmentForNameAsync(firstName, lastName);
            if (appointment == null) {
                return NotFound();
            }
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest();
            }

            appointment = await _appointmentsContext.CreateAppointmentAsync(appointment);

            return CreatedAtRoute("GetAppointment", new { id = appointment.id }, appointment);
        }
    }
}