using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lake_of_the_Humber.Models
{
    //This class describes a product entity.
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public double ProductPrice { get; set; }

        public string ProductDescription { get; set; }

    }

    //This class can be used to transfet information about a product.
    public class ProductDto
    {
        public int ProductID { get; set; }

        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Product Price")]
        public double ProductPrice { get; set; }

        [DisplayName("Product Description")]
        public string ProductDescription { get; set; }
    }
}