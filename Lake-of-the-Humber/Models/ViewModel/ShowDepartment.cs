using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModel
{
    public class ShowDepartment
    {
        public DepartmentDto Department { get; set; }
        public StaffInfoDto staffInfo { get; set; }
    }
}