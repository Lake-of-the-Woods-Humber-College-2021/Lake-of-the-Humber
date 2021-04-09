using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class UpdateAppointment
    {
        //Base information about appointments
        public AppointmentDto appointment { get; set; }

        //displays doctors which could be in a dropdownlist
        public IEnumerable<StaffInfoDto> allstaff { get; set; }
    }
}