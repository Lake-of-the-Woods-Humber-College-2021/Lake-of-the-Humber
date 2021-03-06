using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class ListAppointment
    {
        //This ViewModel is to conditionally render the page based on if the user is an admin or not
        public bool isadmin { get; set; }
        public bool isuser { get; set; }

        public IEnumerable<AppointmentDto> appointment { get; set; }

        public IEnumerable<ApplicationUserDto> users { get; set; }
        public StaffInfoDto staffinfo { get; set; }
    }
}