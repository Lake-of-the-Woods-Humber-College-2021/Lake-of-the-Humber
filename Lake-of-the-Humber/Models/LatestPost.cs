using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models
{
    public class LatestPost
    {
        [Key]
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostSummary { get; set; }
        public string PostCategory { get; set; }
        public DateTime PostDate { get; set; }
        public string PostImagePath { get; set; }
        public string PostContent { get; set; }
        public string Postlink { get; set; }

        [ForeignKey("User")]
        public string PostCreatorId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class LatestPostDto
    {
        public int PostId { get; set; }

        [Required]
        [DisplayName("Post Title")]
        public string PostTitle { get; set; }

        [Required]
        [DisplayName("Post Summary")]
        public string PostSummary { get; set; }

        [Required]
        [DisplayName("Post Category")]
        public string PostCategory { get; set; }

        [Required]
        [DisplayName("Published Date")]
        public DateTime PostDate { get; set; }

        [Required]
        [DisplayName("Photo")]
        public string PostImagePath { get; set; }

        [Required]
        [DisplayName("Post Content")]
        public string PostContent { get; set; }

        [DisplayName("Post Link")]
        public string PostLink { get; set; }

        public string PostCreatorId { get; set; }
    }
}