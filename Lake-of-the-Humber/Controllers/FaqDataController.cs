using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Lake_of_the_Humber.Models.ViewModels;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Data;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Lake_of_the_Humber.Models;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace Lake_of_the_Humber.Controllers
{
    public class FaqDataController : ApiController
    {
        
       
        private ApplicationDbContext db = new ApplicationDbContext();

        //EXAMPLE GET: /api/FaqData/GetFaqs <- returns json of all FAQs
        [ResponseType(typeof(IEnumerable<FaqDto>))]
        //LIST ALL FAQs:
        //Retrieves All Faqs and uses list method to list them.
        public IHttpActionResult GetFaqs()//GETFAQS Faq contoller uses to generate list view
        {
            List<Faq> Faqs = db.Faqs.ToList();
            List<FaqDto> FaqDtos = new List<FaqDto> { };

            //We Want to send the API data so it can infact get the list and present it.
            //The for each loops over all the FAQs to get the details exposed.
            foreach (var Faq in Faqs)
            {
                FaqDto NewFaq = new FaqDto
                {
                    //We are not going to display the creator Id to the public just yet. we want it so only registered users as admin can creat.
                    FaqId = Faq.FaqId,
                    Question = Faq.Question,
                    Answer = Faq.Answer,
                    Publish = Faq.Publish,
                    FaqDate = Faq.FaqDate,

                };
                FaqDtos.Add(NewFaq);
            }

            return Ok(FaqDtos);

        }
        //Method shows the details of a particular FAQ by using the id# to retreive it.
        // Use as EXAMPLE GET: api/FaqData/FindFaq/7
        [HttpGet]
        [ResponseType(typeof(FaqDto))]
        public IHttpActionResult FindFaq(int id)//Finding FAQ by the id...pass in id value to methodmethod name = findFAQ
        {
            //Locate the data of an FAQ
            Faq Faq = db.Faqs.Find(id);
            if (Faq == null)
            {
                return NotFound();//ERROR IF FAQ NOT FOUND
            }
            //Info that API CAn Access
            FaqDto FaqDto = new FaqDto
            {
                FaqId = Faq.FaqId,
                Question = Faq.Question,
                Answer = Faq.Answer,
                Publish = Faq.Publish,
                FaqDate = Faq.FaqDate,
            };
            //return the associated info
            return Ok(FaqDto);
        }
        //DELETE
        //api/FaqData/DeleteFaq/9
        //this needs a get request so they can talk in the faq controller I.E get relevant data info or post the deletion
        //This method uses a POST request to get Faq ID to delete an FAQ BAsed on the id in xml
        [HttpPost]
        public IHttpActionResult DeleteFaq(int id)
        {
            Faq faq = db.Faqs.Find(id);
            if (faq == null)
            {
                return NotFound();//Send Err if id not found in db
            }

            db.Faqs.Remove(faq);
            db.SaveChanges();

            return Ok(faq);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
