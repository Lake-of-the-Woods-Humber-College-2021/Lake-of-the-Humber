using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Lake_of_the_Humber.Models;
using System.Web.Http;
using System.Web.Http.Description;

namespace Lake_of_the_Humber.Controllers
{
    public class FaqDataController : ApiController
    {

        //LIST ALL FAQs:
        private ApplicationDbContext db = new ApplicationDbContext();

        //EXAMPLE GET: /api/FaqData/GetFaqs <- returns json of all FAQs
        [ResponseType(typeof(IEnumerable<FaqDto>))]
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


            //DETAILS OF SINGLE FAQ BY ID#:
            //DELETE ALL FAQS:



        }
    }
}
