using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models
{
    public class InfoSection
    {
        [Key]
        public int SectionId { get; set; }
        public string SectionTitle { get; set; }
        public string SectionDescription { get; set; }
        public int PriorityNumber { get; set; }
        public string link { get; set; }
        public string SectionImageExt {get; set;}

        [ForeignKey("User")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class InfoSectionDto
    {
        public int SectionId { get; set; }

        [Required]
        [DisplayName("Section Title")]
        public string SectionTitle { get; set; }

        [Required]
        [DisplayName("Section Description")]
        public string SectionDescription { get; set; }

        [Required]
        [DisplayName("Priority Number")]
        public int PriorityNumber { get; set; }

        [Required]
        [DisplayName("Link")]
        public string Link { get; set; }

        [Required]
        [DisplayName("Section Image")]
        public string SectionImageExt { get; set; }

        public string CreatorId { get; set; }
    }
}