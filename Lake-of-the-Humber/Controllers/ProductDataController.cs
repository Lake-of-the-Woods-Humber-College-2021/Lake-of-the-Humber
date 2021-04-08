using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Lake_of_the_Humber.Models;
using System.Diagnostics;

namespace Lake_of_the_Humber.Controllers
{
    public class ProductDataController : ApiController
    {
        //This variable is our database access point
        //private GiftShopDataContext db = new GiftShopDataContext();

        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Gets a list or Products in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of Products including their ID, name, price, description.</returns>
        /// <example>
        /// GET: api/ProductData/GetProducts
        /// </example>
        [ResponseType(typeof(IEnumerable<ProductDto>))]
        public IHttpActionResult GetProducts()
        {
            List<Product> Products = db.Products.ToList();
            List<ProductDto> ProductDtos = new List<ProductDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Product in Products)
            {
                ProductDto NewProduct = new ProductDto
                {
                    ProductID = Product.ProductID,
                    ProductName = Product.ProductName,
                    ProductPrice = Product.ProductPrice,
                    ProductDescription = Product.ProductDescription
                };
                ProductDtos.Add(NewProduct);
            }

            return Ok(ProductDtos);
        }

        /// <summary>
        /// Finds a particular Product in the database with a 200 status code. If the Team is not found, return 404.
        /// </summary>
        /// <param name="id">The Product id</param>
        /// <returns>Information about the Product, including Team id, name, price and description</returns>
        // <example>
        // GET: api/ProductData/FindProduct/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult FindProduct(int id)
        {
            //Find the data
            Product Product = db.Products.Find(id);
            //if not found, return 404 status code.
            if (Product == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            ProductDto ProductDto = new ProductDto
            {
                ProductID = Product.ProductID,
                ProductName = Product.ProductName,
                ProductPrice = Product.ProductPrice,
                ProductDescription = Product.ProductDescription
            };


            //pass along data as 200 status code OK response
            return Ok(ProductDto);
        }

        /// <summary>
        /// Gets a list of Order in the database alongside a status code (200 OK).
        /// </summary>
        /// <param name="id">The product id</param>
        /// <returns>A list of Order including their ID, and name and message</returns>
        /// <example>
        /// GET: api/OrderData/GetOrdersForProduct
        /// </example>
        [ResponseType(typeof(IEnumerable<OrderDto>))]
        public IHttpActionResult GetOrdersForProduct(int id)
        {
            List<Order> Orders = db.Orders
                .Where(i => i.Products.Any(p => p.ProductID == id))
                .ToList();
            List<OrderDto> OrderDtos = new List<OrderDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Order in Orders)
            {
                OrderDto NewOrder = new OrderDto
                {
                    OrderID = Order.OrderID,
                    OrderName = Order.OrderName,
                    OrderMessage = Order.OrderMessage
                };
                OrderDtos.Add(NewOrder);
            }

            return Ok(OrderDtos);
        }

        /// <summary>
        /// Updates a Product in the database given information about the Product.
        /// </summary>
        /// <param name="id">The Product id</param>
        /// <param name="Team">A Product object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/ProductData/UpdateProduct/5
        /// FORM DATA: Product JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateProduct(int id, [FromBody] Product Product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Product.ProductID)
            {
                return BadRequest();
            }

            db.Entry(Product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a Product to the database.
        /// </summary>
        /// <param name="Product">A Product object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/ProductData/AddProduct
        ///  FORM DATA: Product JSON Object
        /// </example>
        [ResponseType(typeof(Product))]
        [HttpPost]
        public IHttpActionResult AddProduct([FromBody] Product Product)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(Product);
            db.SaveChanges();

            return Ok(Product.ProductID);
        }

        /// <summary>
        /// Deletes a Product in the database
        /// </summary>
        /// <param name="id">The id of the Product to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/ProductData/DeleteProduct/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product Product = db.Products.Find(id);
            if (Product == null)
            {
                return NotFound();
            }

            db.Products.Remove(Product);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Finds a Product in the system. Internal use only.
        /// </summary>
        /// <param name="id">The Product id</param>
        /// <returns>TRUE if the Product exists, false otherwise.</returns>
        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}