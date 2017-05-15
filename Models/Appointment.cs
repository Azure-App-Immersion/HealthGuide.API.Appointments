using System;

namespace HealthGuide.API.Appointments.Models
{
    public class Appointment
    {        
        public string id { get; set; }
                
        public Patient patient { get; set; }

        public Visit visit { get; set; }

        public DateTime slot { get; set; }

        public Doctor doctor { get; set; }      

        public Location location { get; set; }        
        
        public bool checkedIn { get; set; }

        public bool completed { get; set; }
    }
}