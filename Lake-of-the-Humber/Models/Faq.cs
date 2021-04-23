﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Lake_of_the_Humber.Models
{
    public class Faq
    {
        [Key]
        public int FaqId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool Publish { get; set; }
        public DateTime FaqDate { get; set; }

        [ForeignKey("User")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public class FaqDto
    {
        public int FaqId { get; set; }

        [DisplayName("Question")]
        public string Question { get; set; }

        [DisplayName("Answer")]
        public string Answer { get; set; }

        [DisplayName("Publish")]
        public bool Publish { get; set; }

        [DisplayName("Created On Date")]
        public DateTime FaqDate { get; set; }

        public string CreatorId { get; set; }
    }
}