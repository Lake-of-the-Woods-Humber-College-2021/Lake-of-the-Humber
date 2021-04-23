using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lake_of_the_Humber.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public string OrderName { get; set; }

        public string OrderMessage { get; set; }

        //Utilizes the inverse property to specify the "Many"
        //One Order Many Products
        public ICollection<Product> Products { get; set; }
    }

    public class OrderDto
    {
        public int OrderID { get; set; }

        public string OrderName { get; set; }

        public string OrderMessage { get; set; }
    }
}