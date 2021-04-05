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

namespace Lake_of_the_Humber.Controllers
{
    public class StaffInfoDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StaffInfoData
        public IQueryable<StaffInfo> GetStaffInfoes()
        {
            return db.StaffInfoes;
        }

        // GET: api/StaffInfoData/5
        [ResponseType(typeof(StaffInfo))]
        public IHttpActionResult GetStaffInfo(int id)
        {
            StaffInfo staffInfo = db.StaffInfoes.Find(id);
            if (staffInfo == null)
            {
                return NotFound();
            }

            return Ok(staffInfo);
        }

        // PUT: api/StaffInfoData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStaffInfo(int id, StaffInfo staffInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staffInfo.SatffId)
            {
                return BadRequest();
            }

            db.Entry(staffInfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffInfoExists(id))
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

        // POST: api/StaffInfoData
        [ResponseType(typeof(StaffInfo))]
        public IHttpActionResult PostStaffInfo(StaffInfo staffInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StaffInfoes.Add(staffInfo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = staffInfo.SatffId }, staffInfo);
        }

        // DELETE: api/StaffInfoData/5
        [ResponseType(typeof(StaffInfo))]
        public IHttpActionResult DeleteStaffInfo(int id)
        {
            StaffInfo staffInfo = db.StaffInfoes.Find(id);
            if (staffInfo == null)
            {
                return NotFound();
            }

            db.StaffInfoes.Remove(staffInfo);
            db.SaveChanges();

            return Ok(staffInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StaffInfoExists(int id)
        {
            return db.StaffInfoes.Count(e => e.SatffId == id) > 0;
        }
    }
}