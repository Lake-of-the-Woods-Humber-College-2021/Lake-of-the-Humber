using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lake_of_the_Humber.Models.ViewModels
{
    public class UpdateOrder
    {
        //base information about the order
        public OrderDto order { get; set; }
        //display all products that this order is on
        public IEnumerable<ProductDto> orderedproducts { get; set; }
        //display products which could be ordered in a dropdownlist
        public IEnumerable<ProductDto> allproducts { get; set; }
    }
}