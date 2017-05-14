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
        public async Task<Appointment> Get(string id)
        {
            return await _appointmentsContext.GetAppointmentAsync(id);
        }

        [HttpGet("date/{date}", Name = "GetAppointmentsForDate")]
        public async Task<IEnumerable<Appointment>> Get(DateTimeOffset date)
        {
            return await _appointmentsContext.GetAppointmentsForDateAsync(date);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest();
            }

            await _appointmentsContext.CreateAppointmentAsync(appointment);

            return CreatedAtRoute("GetAppointment", new { id = 0 }, appointment);
        }
    }
}