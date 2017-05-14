using HealthGuide.API.Appointments.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace HealthGuide.API.Appointments.Controllers
{
    [Route("[controller]")]
    [ResponseCache(Duration = 30)]
    public class SlotsController : Controller
    {
        private AppointmentsContext _appointmentsContext;

        public SlotsController(AppointmentsContext appointmentsContext)
        {
            _appointmentsContext = appointmentsContext;
        }

        [HttpGet("{date}", Name = "GetSlots")]
        public IEnumerable<DateTimeOffset> Get(DateTimeOffset date)
        {
            var beginOfDay = date.Date;
            List<DateTimeOffset> dates = new List<DateTimeOffset>
            {
                beginOfDay.AddHours(09.00),
                beginOfDay.AddHours(09.25),
                beginOfDay.AddHours(09.50),
                beginOfDay.AddHours(09.75),
                beginOfDay.AddHours(10.00),
            };
            return dates;
        }
    }
}