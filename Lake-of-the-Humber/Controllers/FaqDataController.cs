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
            List<Faq> Faqs = db.Faqs.Include(faq=>faq.User).ToList();

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
                    CreatorId = Faq.CreatorId
                    //CreatorName = Faq.User.Email

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
            //Info that API Can Access
            FaqDto FaqDto = new FaqDto
            {
                FaqId = Faq.FaqId,
                Question = Faq.Question,
                Answer = Faq.Answer,
                Publish = Faq.Publish,
                FaqDate = Faq.FaqDate,
                CreatorId = Faq.CreatorId,
                CreatorFname = Faq.User.FirstName,
                CreatorLname = Faq.User.LastName,
                FaqHasImage = Faq.FaqHasImage,
                PicExtension = Faq.PicExtension

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

        //ADD/CREATE A NEW FAQ IN THE DB:
        //POST: api/FaqData/AddFaq
        [ResponseType(typeof(Faq))]
        [HttpPost]
        public IHttpActionResult AddFaq([FromBody] Faq Faq)//AddFaq Called on in faqcontroller
        {

            if (!ModelState.IsValid)//Is it acceptable against the db model parameters
            {
                return BadRequest(ModelState);
            }

            db.Faqs.Add(Faq);
            db.SaveChanges();

            return Ok(Faq.FaqId);//Return based on faqId set ID.
        }

        //UPDATE FAQ
        //POST api/FaqData/UpdateFaq/11

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateFaq(int id, [FromBody] Faq Faq)//Update faq called in controller
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Faq.FaqId)//id not eual faq Id
            {
                return BadRequest();
            }

            db.Entry(Faq).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaqExists(id))
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

        //ADD IMAGE TO FAQ:
        public IHttpActionResult UpdateFaqImage(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {

                int numfiles = HttpContext.Current.Request.Files.Count;
                

               
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var FaqImage = HttpContext.Current.Request.Files[0];
                    
                    if (FaqImage.ContentLength > 0)
                    {
                       
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(FaqImage.FileName).Substring(1);
                        
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                
                                string fn = id + "." + extension;

                                
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Faq/"), fn);

                                
                                FaqImage.SaveAs(path);

                                
                                haspic = true;
                                picextension = extension;

                                //Update the player haspic and picextension fields in the database
                                Faq SelectedFaq = db.Faqs.Find(id);
                                SelectedFaq.FaqHasImage = haspic;
                                SelectedFaq.PicExtension = extension;
                                db.Entry(SelectedFaq).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Faq Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                            }
                        }
                    }

                }
            }

            return Ok();
        }


        private bool FaqExists(int id) //called by the update method to insure exsistance of FAQID 
        {
            return db.Faqs.Count(f => f.FaqId == id) > 0;
        }
    }
}
