using System;

namespace HealthGuide.API.Appointments.Models
{
    public class Appointment
    {        
        public string Id { get; set; }
                
        public Patient Patient { get; set; }

        public Visit Visit { get; set; }

        public DateTime Slot { get; set; }

        public Doctor Doctor { get; set; }      

        public Location Location { get; set; }        
        
        public bool CheckedIn { get; set; }

        public bool Completed { get; set; }
    }
}