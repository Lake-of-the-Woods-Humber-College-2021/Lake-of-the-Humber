﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models
{
    public class Appointment
    {
        [Key]
        public int AppId { get; set; }
        public string AppMethod { get; set; }
        public string AppPurpose { get; set; }
        public DateTime AppDate { get; set; }
        public string AppTime { get; set; }

        //An appointment belongs to one user
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        //An appointment belongs to one doctor
        [ForeignKey("StaffInfo")]
        public string StaffId { get; set; }
        public virtual StaffInfo Staff { get; set; }
    }

    public class AppointmentDto
    {
        public int AppId { get; set; }

        [DisplayName("Appointment Method")]
        public string AppMethod { get; set; }

        [DisplayName("Appointment Purpose")]
        public string AppPurpose { get; set; }

        [DisplayName("Appointment Date")]
        public DateTime AppDate { get; set; }

        [DisplayName("Appointment Time")]
        public string AppTime { get; set; }


        public string UserId { get; set; }
        public string StaffId { get; set; }
    }
}