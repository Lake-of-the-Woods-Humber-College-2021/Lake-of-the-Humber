﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModel
{
    public class UpdateDepartment
    {
        public DepartmentDto Department { get; set; }
        public IEnumerable<StaffInfoDto> allstaffs { get; set; }

    }
}