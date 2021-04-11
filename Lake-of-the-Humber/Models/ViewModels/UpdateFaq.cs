using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class UpdateFaq
    {
        public FaqDto Faq { get; set; }
        /*  public CreatorDto Creator { get; set; }*///EVENTUALLY MAKE USABILITY BASED ON USER BEING ADMIN ETC
    }
}