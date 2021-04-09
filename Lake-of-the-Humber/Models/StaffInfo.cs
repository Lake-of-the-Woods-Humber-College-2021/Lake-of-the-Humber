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
        public int SatffId { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public string StaffLanguage { get; set; }
        public bool StaffHasPic { get; set; }
        public string StaffImagePath { get; set; } // if empty, it will replaced with default image

        //A Staff belong to one department
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public virtual Department Department { get; set; }

        [ForeignKey("User")]
        public string StaffCreatorId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //A staff can have many appointments
        public ICollection<Appointment> Appointment { get; set; }

    }

    public class StaffInfoDto
    {
        public int StaffId { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string StaffFirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string StaffLastName { get; set; }

        [Required]
        [DisplayName("Language(s)")]
        public string StaffLanguage { get; set; }
        public bool StaffHasPic { get; set; }

        [DisplayName("Photo")]
        public string StaffImagePath { get; set; }

        public string StaffCreatorId { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

}