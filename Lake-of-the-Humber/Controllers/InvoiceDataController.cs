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
        public IHttpActionResult GetInvoices()
        {
            List<Invoice> Invoices = db.Invoices.ToList();
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

            Debug.WriteLine("Successful grabbing of list");
            Debug.WriteLine(DateTime.Now);
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
        public IHttpActionResult AddInvoice([FromBody] Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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