using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Lake_of_the_Humber.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentAddress { get; set; }
        public string DepartmentPhone { get; set; }

        [ForeignKey("User")]
        public string DepartmentCreatorId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //A department can place many staffs
        public ICollection<StaffInfo> Staffs { get; set; }

    }

    public class DepartmentDto
    {
        public int DepartmentId { get; set; }

        [Required]
        [DisplayName("Department Name")]
        public string DepartmentName { get; set; }

        [DisplayName("Department Address")]
        public string DepartmentAddress { get; set; }

        [DisplayName("Department Phone Number")]
        public string DepartmentPhone { get; set; }

        public string StaffCreatorId { get; set; }
    }
}