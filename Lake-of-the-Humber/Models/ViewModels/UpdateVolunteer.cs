using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class UpdateVolunteer
    {
        public VolunteerDto Volunteer { get; set; }
        /*  public CreatorDto Creator { get; set; }*///EVENTUALLY MAKE USABILITY BASED ON USER BEING ADMIN ETC
    }
}