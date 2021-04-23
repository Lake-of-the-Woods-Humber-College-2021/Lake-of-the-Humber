using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels.WellWishes
{
    public class ListWishes
    {
        public bool isadmin { get; set; }
        public IEnumerable<WellWishDto> wishes { get; set; }
    }
}