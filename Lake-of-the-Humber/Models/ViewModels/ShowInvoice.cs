using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class ShowInvoice
    {

        public bool isadmin { get; set; }
        public bool isuser { get; set; }
        public InvoiceDto invoice { get; set; }

        //Information about user associated with an appointment
        public virtual ApplicationUser userInfo { get; set; }

    }
}