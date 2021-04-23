using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Lake_of_the_Humber.Models;

namespace Lake_of_the_Humber.Controllers
{
    public class StaffInfoDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets a list of staffs in the database alongside a status code (200 OK)
        /// </summary>
        /// <returns>A list of  staffs with their information including first and last name, languages, hasPic, and Pic Extension(Image path)</returns>
        ///<example> GET: api/StaffInfoData/GetStaffInfos </example>
        [ResponseType(typeof(IEnumerable<StaffInfoDto>))]
        public IHttpActionResult GetStaffInfos()
        {
            List<StaffInfo> StaffInfoes = db.StaffInfoes.ToList();
            List<StaffInfoDto> StaffInfoDtos = new List<StaffInfoDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var StaffInfo in StaffInfoes)
            {
                Department Department = db.Departments
               .Where(d => d.StaffInfoes.Any(s => s.DepartmentID == StaffInfo.DepartmentID))
               .FirstOrDefault();

                //if not found, return 404 status code.
                if (Department == null)
                {
                    return NotFound();
                }
                StaffInfoDto NewStaffInfo = new StaffInfoDto
                {
                    StaffID = StaffInfo.StaffID,
                    StaffFirstName = StaffInfo.StaffFirstName,
                    StaffLastName = StaffInfo.StaffLastName,
                    StaffLanguage = StaffInfo.StaffLanguage,
                    StaffHasPic = StaffInfo.StaffHasPic,
                    StaffImagePath = StaffInfo.StaffImagePath,
                    DepartmentID = StaffInfo.DepartmentID

                };
                StaffInfoDtos.Add(NewStaffInfo);
            }

            return Ok(StaffInfoDtos);

        }


        /// <summary>
        /// Finds a particular Staff in the database with a 200 status code. If the staff is not found, return 404.
        /// </summary>
        /// <param name="id">The stafft id</param>
        /// <returns>Information about the selected staff information including first and last name, languages, hasPic, and Pic Extension(Image path)</returns>
        // <example>
        // GET: api/StaffInfoData/FindStaff/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(StaffInfoDto))]
        public IHttpActionResult FindStaff(int id)
        {
            //Find a staffInfo Data
            StaffInfo StaffInfo = db.StaffInfoes.Find(id);
            //If not found, return 404 status code
            if (StaffInfo == null)
            {
                return NotFound();
            }
            Department Department = db.Departments
               .Where(d => d.StaffInfoes.Any(s => s.DepartmentID == StaffInfo.DepartmentID))
               .FirstOrDefault();

            //if not found, return 404 status code.
            if (Department == null)
            {
                return NotFound();
            }
            StaffInfoDto StaffInfoDto = new StaffInfoDto
            {
                StaffID = StaffInfo.StaffID,
                StaffFirstName = StaffInfo.StaffFirstName,
                StaffLastName = StaffInfo.StaffLastName,
                StaffLanguage = StaffInfo.StaffLanguage,
                StaffHasPic = StaffInfo.StaffHasPic,
                StaffImagePath = StaffInfo.StaffImagePath,
                DepartmentID = StaffInfo.DepartmentID

            };

            //pass along data as 200 status code OK response
            return Ok(StaffInfoDto);
        }
        /// <summary>
        /// //////////////////NEEED TO CHECK ON THIS OUTCOME IS DEPARTMENT ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // <example>
        // GET: api/StaffInfoData/FindDepartmentForStaff/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult FindDepartmentForStaff(int id)
        {

            Department Department = db.Departments
                .Where(d => d.StaffInfoes.Any(s => s.DepartmentID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Department == null)
            {
                return NotFound();
            }

            DepartmentDto DepartmentDto = new DepartmentDto
            {
                DepartmentID = Department.DepartmentId,
                DepartmentName = Department.DepartmentName,
                DepartmentAddress = Department.DepartmentAddress,
                DepartmentPhone = Department.DepartmentPhone
            };


            //pass along data as 200 status code OK response
            return Ok(DepartmentDto);
        }


        /// <summary>
        /// Updates staff information after any changes apply.
        /// </summary>
        /// <param name="id">The staff id</param>
        /// <param name="StaffInfo">A StaffInfo object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/StaffInfoData/UpdateStaff/5
        /// FORM DATA: StaffInfo JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateStaff(int id, [FromBody] StaffInfo StaffInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != StaffInfo.StaffID)
            {
                return BadRequest();
            }

            db.Entry(StaffInfo).State = EntityState.Modified;
            // Picture update is handled by another method
            db.Entry(StaffInfo).Property(s => s.StaffHasPic).IsModified = false;
            db.Entry(StaffInfo).Property(s => s.StaffImagePath).IsModified = false;

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

        /// <summary>
        /// Receives Staff picture data, uploads it to the webserver and updates the staff's HasPic option
        /// </summary>
        /// <param name="id">the staff id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F staffpic=@file.jpg "https://localhost:xx/api/StaffInfodata/updatestaffpic/2"
        /// POST: api/StaffInfoData/UpdateStaffPic/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        public IHttpActionResult UpdateStaffPic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {

                int numfiles = HttpContext.Current.Request.Files.Count;

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var StaffPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (StaffPic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(StaffPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/Products/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/StaffInfoes/"), fn);

                                //save the file
                                StaffPic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the product haspic and picextension fields in the database
                                StaffInfo SelectedStaffInfo = db.StaffInfoes.Find(id);
                                SelectedStaffInfo.StaffHasPic = haspic;
                                SelectedStaffInfo.StaffImagePath = extension;
                                db.Entry(SelectedStaffInfo).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Staff Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                            }
                        }
                    }

                }
            }

            return Ok();
        }

        /// <summary>
        /// Adds a StaffInfo to the database.
        /// </summary>
        /// <param name="StaffInfo">A StaffInfo object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/StaffInfoData/AddStaff
        ///  FORM DATA: StaffInfo JSON Object
        /// </example>
        [ResponseType(typeof(StaffInfo))]
        [HttpPost]
        public IHttpActionResult AddStaff([FromBody] StaffInfo StaffInfo)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StaffInfoes.Add(StaffInfo);
            db.SaveChanges();

            return Ok(StaffInfo.StaffID);
        }

        /// <summary>
        /// Deletes a StaffInfo in the database
        /// </summary>
        /// <param name="id">The id of the staff to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/StaffInfoData/DeleteStaff/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteStaff(int id)
        {
            StaffInfo StaffInfo = db.StaffInfoes.Find(id);
            if (StaffInfo == null)
            {
                return NotFound();
            }
            if (StaffInfo.StaffHasPic && StaffInfo.StaffImagePath != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/StaffInfoes/" + id + "." + StaffInfo.StaffImagePath);
                if (System.IO.File.Exists(path))
                {

                    System.IO.File.Delete(path);
                }
            }

            db.StaffInfoes.Remove(StaffInfo);
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
        /// Finds a product in the system. Internal use only.
        /// </summary>
        /// <param name="id">The product id</param>
        /// <returns>TRUE if the product exists, false otherwise.</returns>
        private bool StaffInfoExists(int id)
        {
            return db.StaffInfoes.Count(e => e.StaffID == id) > 0;
        }
    }
}