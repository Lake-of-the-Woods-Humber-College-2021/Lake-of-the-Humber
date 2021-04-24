using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels.InfoSections
{
    public class UpdateInfoSection
    {
        public InfoSectionDto section { get; set; }

        public IEnumerable<DepartmentDto> allDepartments{ get; set; }
    }
}