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
        public DateTime ReceivedDate { get; set; }

        [ForeignKey("User")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public class WellWishDto
    {
        public int WishId { get; set; }

        [Required]
        [DisplayName("Message")]
        public string Message { get; set; }

        [Required]
        [DisplayName("Room Number")]
        public string RoomNumber { get; set; }

        [Required]
        [DisplayName("Room Number")]
        public string ReceiverName { get; set; }

        public string CreatorId { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Is Received")]
        public bool IsReceived { get; set; }

        [DisplayName("Received Date")]
        public DateTime ReceivedDate { get; set; }
    }
}