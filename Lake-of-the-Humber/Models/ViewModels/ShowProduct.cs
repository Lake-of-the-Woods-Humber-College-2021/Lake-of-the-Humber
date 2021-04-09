using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class ShowProduct
    {
        public ProductDto product { get; set; }

        //Information about all orders for that product
        public IEnumerable<OrderDto> productorders { get; set; }
    }
}