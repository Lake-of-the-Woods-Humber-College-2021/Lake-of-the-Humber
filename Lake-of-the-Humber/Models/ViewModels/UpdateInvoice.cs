using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class UpdateInvoice
    {

        public InvoiceDto invoice { get; set; }

        public virtual ApplicationUser user { get; set; }


    }
}