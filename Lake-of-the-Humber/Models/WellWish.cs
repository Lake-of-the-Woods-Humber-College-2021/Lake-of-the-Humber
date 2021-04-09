using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models
{
    public class WellWish
    {
        [Key]
        public int WishId { get; set; }
        public string Message { get; set; }
        public string RoomNumber { get; set; }
        public string ReceiverName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsReceived { get; set; }
        public Nullable<DateTime> ReceivedDate { get; set; }

        [ForeignKey("User")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }


    public class WellWishDto
    {
        public int WishId { get; set; }

        [Required(ErrorMessage = "Enter Receiver Name")]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        [DisplayName("Receiver Name")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Enter Room Number")]
        [StringLength(5, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        [DisplayName("Room Number")]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = "Enter Message")]
        [StringLength(255, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 25)]
        [DisplayName("Message")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        public string CreatorId { get; set; }


        [DisplayName("Delivered")]
        public bool IsReceived { get; set; }

        [DisplayName("Received Date")]
        public Nullable<DateTime> ReceivedDate { get; set; }
    }
}