using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Lake_of_the_Humber.Models
{
    public class StaffInfo
    {
        [Key]
        public int StaffID { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public string StaffLanguage { get; set; }
        public bool StaffHasPic { get; set; }
        public string StaffImagePath { get; set; } // if empty, it will replaced with default image

        //A Staff belong to one department
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //A staff can have many appointments
        public ICollection<Appointment> Appointment { get; set; }

    }

    public class StaffInfoDto
    {
        public int StaffID { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "Please Enter Staff's First Name.")]
        public string StaffFirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Please Enter Staff's Last Name.")]
        public string StaffLastName { get; set; }

        [DisplayName("Language(s)")]
        [Required(ErrorMessage = "Please Enter Minimum One Language ( i.e. English).")]
        public string StaffLanguage { get; set; }
        public bool StaffHasPic { get; set; }

        [DisplayName("Photo")]
        public string StaffImagePath { get; set; }

        public string UserId { get; set; }

        public int DepartmentID { get; set; }

        public string DepartmentName { get; set; }
        public string DepartmentAddress { get; set; }
        public string DepartmentPhone { get; set; }
    }

}
