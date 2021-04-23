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
        public string Link { get; set; }
        public string LinkBtnName { get; set; }
        public string SectionImageExt {get; set;}
        public bool IsArchive {get; set; }

        [ForeignKey("User")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class InfoSectionDto
    {
        public int SectionId { get; set; }

        [Required(ErrorMessage = "Enter Section Title")]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        [DisplayName("Section Title")]
        public string SectionTitle { get; set; }

        [Required(ErrorMessage = "Enter Section Title")]
        [StringLength(255, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 5)]
        [DisplayName("Section Description")]
        public string SectionDescription { get; set; }

        [Required(ErrorMessage = "Enter Priority Number")]
        [DisplayName("Priority Number")]
        public int PriorityNumber { get; set; }

        [Url(ErrorMessage = "Enter Valid Link")]
        [DisplayName("Link")]
        public string Link { get; set; }
       
        [DisplayName("Link Button Name")]
        public string LinkBtnName { get; set; }

        [DisplayName("Section Image")]
        public string SectionImageExt { get; set; }

        [DisplayName("Archive")]
        public bool IsArchive { get; set; }
        public string CreatorId { get; set; }
    }
}