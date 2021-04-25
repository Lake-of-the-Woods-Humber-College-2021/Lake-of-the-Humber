using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public string InvoiceTitle { get; set; }
        public string InvoiceDesc { get; set; }
        public DateTime InvoiceDate { get; set; }

        public decimal InvoiceCost { get; set; }
        public bool IsPaid { get; set; }

        //An invoice belongs to one user
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class InvoiceDto
    {
        public int InvoiceId { get; set; }

        [DisplayName("Invoice Title")]
        public string InvoiceTitle { get; set; }

        [DisplayName("Invoice Description")]
        public string InvoiceDesc { get; set; }

        [DisplayName("Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        public string DInvoiceDate { get; set; }

        [DisplayName("Invoice Cost")]
        public decimal InvoiceCost { get; set; }


        public bool IsPaid { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}