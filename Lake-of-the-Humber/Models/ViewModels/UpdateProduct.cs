using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    //The View Model required to update a product
    public class UpdateProduct
    {
        //Information about the product
        public ProductDto product { get; set; }
    }
}