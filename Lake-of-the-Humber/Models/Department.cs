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
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //A department can place many staffs
        public ICollection<StaffInfo> StaffInfoes { get; set; }

        //A department can have many information sections related to them
        public ICollection<InfoSection> InfoSections { get; set; }

    }

    public class DepartmentDto
    {
        public int DepartmentId { get; set; }

        [DisplayName("Department Name")]
        [Required(ErrorMessage = "Please Enter a Department Name.")]
        public string DepartmentName { get; set; }

        [DisplayName("Department Address")]
        [Required(ErrorMessage = "Please Enter Department's Address (i.e. Clinic A - Green Owl Zone (Level 1).")]
        public string DepartmentAddress { get; set; }

        [DisplayName("Department Phone Number")]
        [Required(ErrorMessage = "Please Enter Department's Phone Number ( i.e. 647-222-3333).")]
        public string DepartmentPhone { get; set; }

        public string UserId { get; set; }


    }
}
