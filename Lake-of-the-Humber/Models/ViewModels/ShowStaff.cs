using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class ShowStaff
    {
        public StaffInfoDto staffinfo { get; set; }
        public DepartmentDto department { get; set; }
        public virtual ApplicationUser userInfo { get; set; }
    }
}