using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class ShowAppointment
    {
        public AppointmentDto appointment { get; set; }

        //Information about the staff associated with an appointment
        public StaffInfoDto staffInfo { get; set; }

        //Information about user associated with an appointment
        public virtual ApplicationUser userInfo { get; set; }
    }
}