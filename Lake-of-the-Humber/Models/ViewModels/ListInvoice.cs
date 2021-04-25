using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class ListInvoice
    {
        //This ViewModel is to conditionally render the page based on if the user is an admin or not
        public bool isadmin { get; set; }
        public bool isuser { get; set; }

        public IEnumerable<InvoiceDto> invoices { get; set; }
        public IEnumerable<ApplicationUserDto> users { get; set; }
    }
}