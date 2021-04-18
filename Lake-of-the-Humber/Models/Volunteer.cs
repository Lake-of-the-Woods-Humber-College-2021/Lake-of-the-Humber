using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerId { get; set; }
        public string VolunteerTitle { get; set; }
        public string VolunteerDescription { get; set; }
        public bool PublishVolunteer { get; set; }
        public DateTime VolunteerDate { get; set; }

        [ForeignKey("User")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class VolunteerDto
    {
        public int VolunteerId { get; set; }

        [DisplayName("Volunteer Position Title")]
        public string VolunteerTitle { get; set; }

        [DisplayName("Volunteer Position Description")]
        public string VolunteerDescription { get; set; }

        [DisplayName("Publish Volunteer Position")]
        public bool PublishVolunteer { get; set; }

        [DisplayName("Created On")]
        public DateTime VolunteerDate { get; set; }

        public string CreatorId { get; set; }
    }
}