using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class ListAppointment
    {

        public IEnumerable<AppointmentDto> appointment { get; set; }

        public IEnumerable<StaffInfoDto> staffinfo { get; set; }
    }
}