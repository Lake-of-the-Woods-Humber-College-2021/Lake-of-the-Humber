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
    public class VolunteerDataController : ApiController
    {
        // GET: VolunteerData

        private ApplicationDbContext db = new ApplicationDbContext();

        //EXAMPLE GET: /api/VolunteerData/GetVolunteers <- returns json of all Volunteers
        [ResponseType(typeof(IEnumerable<VolunteerDto>))]
        //LIST ALL Volunteers:
        //Retrieves All Volunteers and uses list method to list them.
        public IHttpActionResult GetVolunteers()//GETVolunteerS Volunteer contoller uses to generate list view
        {
            List<Volunteer> Volunteers = db.Volunteers.ToList();
            List<VolunteerDto> VolunteerDtos = new List<VolunteerDto> { };

            //We Want to send the API data so it can infact get the list and present it.
            //The for each loops over all the Volunteers to get the details exposed.
            foreach (var Volunteer in Volunteers)
            {
                VolunteerDto NewVolunteer = new VolunteerDto
                {
                    //We are not going to display the creator Id to the public just yet. we want it so only registered users as admin can creat.
                    VolunteerId = Volunteer.VolunteerId,
                    VolunteerTitle = Volunteer.VolunteerTitle,
                    VolunteerDescription = Volunteer.VolunteerDescription,
                    PublishVolunteer = Volunteer.PublishVolunteer,
                    VolunteerDate = Volunteer.VolunteerDate,
                    CreatorId = Volunteer.CreatorId

                };
                VolunteerDtos.Add(NewVolunteer);
            }

            return Ok(VolunteerDtos);

        }
        //DETAILS:
        //Method shows the details of a particular Volunteer by using the id# to retreive it.
        // Use as EXAMPLE GET: api/VolunteerData/FindVolunteer/7
        [HttpGet]
        [ResponseType(typeof(VolunteerDto))]
        public IHttpActionResult FindVolunteer(int id)//Finding Volunteer by the id...pass in id value to methodmethod name = findFAQ
        {
            //Locate the data of an Volunteer
            Volunteer Volunteer = db.Volunteers.Find(id);
            if (Volunteer == null)
            {
                return NotFound();//ERROR IF Volunteer NOT FOUND
            }
            //Info that API CAn Access
            VolunteerDto VolunteerDto = new VolunteerDto
            {
                VolunteerId = Volunteer.VolunteerId,
                VolunteerTitle = Volunteer.VolunteerTitle,
                VolunteerDescription = Volunteer.VolunteerDescription,
                PublishVolunteer = Volunteer.PublishVolunteer,
                VolunteerDate = Volunteer.VolunteerDate,
                CreatorId = Volunteer.CreatorId
            };
            //return the associated info
            return Ok(VolunteerDto);
        }

        //DELETE
        //api/VolunteerData/DeleteVolunteer/9
        //this needs a get request so they can talk in the Volunteer controller I.E get relevant data info or post the deletion
        //This method uses a POST request to get Volunteer ID to delete an Volunteer BAsed on the id in xml
        [HttpPost]
        public IHttpActionResult DeleteVolunteer(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return NotFound();//Send Err if id not found in db
            }

            db.Volunteers.Remove(volunteer);
            db.SaveChanges();

            return Ok(volunteer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //ADD/CREATE A NEW Volunteer IN THE DB:
        //POST: api/VolunteerData/AddVolunteer
        [ResponseType(typeof(Volunteer))]
        [HttpPost]
        public IHttpActionResult AddVolunteer([FromBody] Volunteer Volunteer)//AddVolunteer Called on in faqcontroller
        {

            if (!ModelState.IsValid)//Is it acceptable against the db model parameters
            {
                return BadRequest(ModelState);
            }

            db.Volunteers.Add(Volunteer);
            db.SaveChanges();

            return Ok(Volunteer.VolunteerId);//Return based on VolunteerId set ID.
        }

        //UPDATE Volunteer
        //POST api/VolunteerData/UpdateVolunteer/4

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateVolunteer(int id, [FromBody] Volunteer Volunteer)//Update faq called in controller
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Volunteer.VolunteerId)//id not eual Volunteer Id
            {
                return BadRequest();
            }

            db.Entry(Volunteer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolunteerExists(id))
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
        private bool VolunteerExists(int id) //called by the update method to insure exsistance of FAQID 
        {
            return db.Volunteers.Count(f => f.VolunteerId == id) > 0;
        }

    }
}