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
    public class OrderDataController : ApiController
    {
        //This variable is our database access point
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets a list of Orders in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of Orders including their ID, and name and message .</returns>
        /// <example>
        /// GET: api/OrderData/GetOrders
        /// </example>
        [ResponseType(typeof(IEnumerable<OrderDto>))]
        public IHttpActionResult GetOrders()
        {
            List<Order> Orders = db.Orders.ToList();
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
        /// Gets a list or Products in the database associated with a particular order. Returns a status code (200 OK)
        /// </summary>
        /// <param name="id">The input order id</param>
        /// <returns>A list of Products including their ID, name, price and description</returns>
        [ResponseType(typeof(IEnumerable<ProductDto>))]
        public IHttpActionResult GetProductsForOrder(int id)
        {
            //sql equivalent
            //select * from Products
            //inner join productorders on productorders.productid = products.productid
            //inner join orders on orders.orderid = productorders.orderid
            List<Product> Products = db.Products
                .Where(p => p.Orders.Any(i => i.OrderID == id))
                .ToList();
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
        /// Gets a list or Products in the database NOT associated with a order. These could be potentially sponsored teams.
        /// </summary>
        /// <param name="id">The input order id</param>
        /// <returns>A list of Product including their ID, name, price and description</returns>
        /// <example>
        /// GET: api/ProductData/GetProductsForOrder
        /// </example>
        [ResponseType(typeof(IEnumerable<ProductDto>))]
        public IHttpActionResult GetProductsNotOrdered(int id)
        {
            List<Product> Products = db.Products
                .Where(p => !p.Orders.Any(i => i.OrderID == id))
                .ToList();
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
        /// Finds a particular Order in the database with a 200 status code. If the Order is not found, return 404.
        /// </summary>
        /// <param name="id">The Order id</param>
        /// <returns>Information about the Order, including Order id, name and message</returns>
        // <example>
        // GET: api/OrderData/FindOrder/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(OrderDto))]
        public IHttpActionResult FindOrder(int id)
        {
            //Find the data
            Order Order = db.Orders.Find(id);
            //if not found, return 404 status code.
            if (Order == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            OrderDto OrderDto = new OrderDto
            {
                OrderID = Order.OrderID,
                OrderName = Order.OrderName,
                OrderMessage = Order.OrderMessage
            };


            //pass along data as 200 status code OK response
            return Ok(OrderDto);
        }

        /// <summary>
        /// Updates a Order in the database given information about the Order.
        /// </summary>
        /// <param name="id">The Order id</param>
        /// <param name="order">A Order object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/OrderData/UpdateOrder/5
        /// FORM DATA:Order JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateOrder(int id, [FromBody] Order Order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Order.OrderID)
            {
                return BadRequest();
            }

            db.Entry(Order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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
        /// Adds a Order to the database.
        /// </summary>
        /// <param name="Order">A Order object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/Orders/AddOrder
        ///  FORM DATA:Order JSON Object
        /// </example>
        [ResponseType(typeof(Order))]
        [HttpPost]
        public IHttpActionResult AddOrder([FromBody] Order Order)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(Order);
            db.SaveChanges();

            return Ok(Order.OrderID);
        }

        /// <summary>
        /// Deletes a Order in the database
        /// </summary>
        /// <param name="id">The id of the Order to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/Orders/DeleteOrder/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order Order = db.Orders.Find(id);
            if (Order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(Order);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Deletes a relationship between a particular product and a Order
        /// </summary>
        /// <param name="productid">The product id</param>
        /// <param name="Orderid">The Order  id</param>
        /// <returns>status code of 200 OK</returns>
        [HttpGet]
        [Route("api/orderdata/unorder/{productid}/{orderid}")]
        public IHttpActionResult Unorder(int productid, int orderid)
        {
            //First select the Order (also loading in product data)
            Order SelectedOrder = db.Orders
                .Include(i => i.Products)
                .Where(i => i.OrderID == orderid)
                .FirstOrDefault();

            //Then select the product
            Product SelectedProduct = db.Products.Find(productid);

            if (SelectedOrder == null || SelectedProduct == null || !SelectedOrder.Products.Contains(SelectedProduct))
            {

                return NotFound();
            }
            else
            {
                //Remove the Order from the product
                SelectedOrder.Products.Remove(SelectedProduct);
                db.SaveChanges();
                return Ok();
            }
        }

        /// <summary>
        /// Adds a relationship between a particular product and an Order
        /// </summary>
        /// <param name="productid">The product id</param>
        /// <param name="orderid">The Order id</param>
        /// <returns>status code of 200 OK</returns>
        [HttpGet]
        [Route("api/orderdata/order/{productid}/{orderid}")]
        public IHttpActionResult Order(int productid, int orderid)
        {
            //First select the Order (also loading in product data)
            Order SelectedOrder = db.Orders
                .Include(i => i.Products)
                .Where(i => i.OrderID == orderid)
                .FirstOrDefault();

            //Then select the product
            Product SelectedProduct = db.Products.Find(productid);

            if (SelectedOrder == null || SelectedProduct == null || SelectedOrder.Products.Contains(SelectedProduct))
            {

                return NotFound();
            }
            else
            {
                //Add the order from the product
                SelectedOrder.Products.Add(SelectedProduct);
                db.SaveChanges();
                return Ok();
            }
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
        /// Finds a Order in the system. Internal use only.
        /// </summary>
        /// <param name="id">The Order id</param>
        /// <returns>TRUE if the Order exists, false otherwise.</returns>
        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.OrderID == id) > 0;
        }

        private bool OrderAssociated(int productid, int orderid)
        {
            return true;
        }
    }
}