using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Lake_of_the_Humber.Models;
using System.Diagnostics;

namespace Lake_of_the_Humber.Controllers
{
    public class InvoiceDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets a list of invoices in the database along with status code
        /// </summary>
        /// <returns>A list of invoices</returns>
        /// <example>
        /// GET: api/InvoiceData/GetInvoices
        /// </example>
        [ResponseType(typeof(IEnumerable<InvoiceDto>))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetInvoices()
        {
            List<Invoice> Invoices = db.Invoices.Include(i=>i.User).ToList();
            List<InvoiceDto> InvoiceDtos = new List<InvoiceDto> { };

            foreach (var Invoice in Invoices)
            {
                InvoiceDto NewInvoice = new InvoiceDto
                {
                    InvoiceId = Invoice.InvoiceId,
                    InvoiceTitle = Invoice.InvoiceTitle,
                    InvoiceDesc = Invoice.InvoiceDesc,
                    InvoiceDate = Invoice.InvoiceDate,
                    DInvoiceDate = Invoice.InvoiceDate.ToString("MMM d yyyy"),
                    InvoiceCost = Invoice.InvoiceCost,
                    IsPaid = Invoice.IsPaid,
                    UserId = Invoice.UserId,
                    FirstName = Invoice.User.FirstName,
                    LastName = Invoice.User.LastName
                };
                InvoiceDtos.Add(NewInvoice);
            }

            Debug.WriteLine("Successful grabbing of list");
            Debug.WriteLine(DateTime.Now);
            //Passes data as 200 status code
            return Ok(InvoiceDtos);

        }


        /// <summary>
        /// Gets a list of invoices associated with a logged in user
        /// </summary>
        /// <param name="id">The logged in UserId</param>
        /// <returns>A list of invoices associated with a logged in user</returns>
        /// <example>
        /// GET: api/InvoiceData/GetInvoicesForUser/abcdef-12345-ghijkl
        /// </example>

        [Authorize(Roles = "User")]
        public IHttpActionResult GetInvoicesForUser(string id)
        {
            IEnumerable<Invoice> Invoices = db.Invoices.Where(i => i.UserId == id);
            List<InvoiceDto> InvoiceDtos = new List<InvoiceDto> { };

            foreach (var Invoice in Invoices)
            {
                InvoiceDto NewInvoice = new InvoiceDto
                {
                    InvoiceId = Invoice.InvoiceId,
                    InvoiceTitle = Invoice.InvoiceTitle,
                    InvoiceDesc = Invoice.InvoiceDesc,
                    InvoiceDate = Invoice.InvoiceDate,
                    DInvoiceDate = Invoice.InvoiceDate.ToString("MMM d yyyy"),
                    InvoiceCost = Invoice.InvoiceCost,
                    IsPaid = Invoice.IsPaid,
                    UserId = Invoice.UserId
                };
                InvoiceDtos.Add(NewInvoice);
            }

            //Passes data as 200 status code
            return Ok(InvoiceDtos);

        }

        /// <summary>
        /// Get a single invoice in database with a 200 status code. Returns 404 status code if invoice is not found
        /// </summary>
        /// <param name="id">The invoice ID</param>
        /// <returns>Information about the invoice (title, description, cost, etc.) </returns>
        /// <example>
        /// GET: api/InvoiceData/FindInvoice/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(InvoiceDto))]
        [Authorize(Roles = "Admin,User")]
        public IHttpActionResult FindInvoice(int id)
        {
            //Finds data
            Invoice Invoice = db.Invoices.Find(id);
            //If data is not found, returns 404 status code
            if (Invoice == null)
            {
                return NotFound();
            }

            InvoiceDto InvoiceDto = new InvoiceDto
            {
                InvoiceId = Invoice.InvoiceId,
                InvoiceTitle = Invoice.InvoiceTitle,
                InvoiceDesc = Invoice.InvoiceDesc,
                InvoiceDate = Invoice.InvoiceDate,
                DInvoiceDate = Invoice.InvoiceDate.ToString("MMM d yyyy"),
                InvoiceCost = Invoice.InvoiceCost,
                IsPaid = Invoice.IsPaid,
                UserId = Invoice.UserId
            };

            //Passes data as 200 status code
            return Ok(InvoiceDto);
        }

        /// <summary>
        /// Finds user that is associated with particular invoice
        /// </summary>
        /// <param name="id">The invoice id</param>
        /// <returns>Information about the user associated with invoice</returns>
        /// <example>
        /// GET: api/InvoiceData/FindUserForInvoice/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(InvoiceDto))]
        public IHttpActionResult FindUserForInvoice(int id)
        {
            //Find data
            ApplicationUser User = db.Users.Where(u => u.Invoice.Any(a => a.InvoiceId == id)).FirstOrDefault();
            if (User == null)
            {
                return NotFound();
            }

            ApplicationUser UserDto = new ApplicationUser
            {
                UserName = User.UserName,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email
            };
            return Ok(UserDto);

        }


        /// <summary>
        /// Gets a list of users in the database alongside a status code (200 OK)
        /// </summary>
        /// <returns>A list of Users in the database with their info</returns>
        /// <example>
        /// GET: api/InvoiceData/GetUsers
        /// </example>
        [ResponseType(typeof(IEnumerable<ApplicationUserDto>))]
        public IHttpActionResult GetUsers()
        {
            List<ApplicationUser> Users = db.Users.ToList();
            List<ApplicationUserDto> UserDtos = new List<ApplicationUserDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var User in Users)
            {
                ApplicationUserDto NewUser = new ApplicationUserDto
                {
                    Id = User.Id,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    UserName = User.UserName,
                    Email = User.Email
               
                };
                UserDtos.Add(NewUser);
            }

            return Ok(UserDtos);
        }

        /// <summary>
        /// Update an invoice in the database given information about the invoice
        /// </summary>
        /// <param name="id">Invoice Id</param>
        /// <param name="invoice">An Invoice Object. Received as POST data</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/InvoiceData/UpdateInvoice/5
        /// FORM DATA: Invoice JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateInvoice(int id, [FromBody] Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invoice.InvoiceId)
            {
                return BadRequest();
            }

            db.Entry(invoice).State = EntityState.Modified;

            db.Entry(invoice).Property(p => p.InvoiceDate).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
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
        /// Adds an invoice to the database
        /// </summary>
        /// <param name="invoice">An invoice Object. Sent as Post Form data.</param>
        /// <returns>Status Code 200 if successful, 400 in unsuccessful</returns>
        /// <example>
        /// FORM DATA: Invoice JSON Object
        /// </example>
        [ResponseType(typeof(Invoice))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddInvoice([FromBody] Invoice invoice) //,[FromBody] ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            invoice.InvoiceDate = DateTime.Now;
            //invoice.UserId = user.Id;

            db.Invoices.Add(invoice);
            db.SaveChanges();

            return Ok(invoice.InvoiceId);
        }

        /// <summary>
        /// Deletes invoice in the database
        /// </summary>
        /// <param name="id">The id of the invoice to delete</param>
        /// <returns>200 if successful, 400 in unsuccessful.</returns>
        /// <example>
        /// POST: api/InvoiceData/DeleteInvoice/5
        /// </example> 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteInvoice(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return NotFound();
            }

            db.Invoices.Remove(invoice);
            db.SaveChanges();

            return Ok(invoice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InvoiceExists(int id)
        {
            return db.Invoices.Count(e => e.InvoiceId == id) > 0;
        }
    }
}