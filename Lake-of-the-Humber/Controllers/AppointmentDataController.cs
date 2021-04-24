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
using Microsoft.AspNet.Identity;

namespace Lake_of_the_Humber.Controllers
{
    public class AppointmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets a list of appointments in the database along with status code
        /// </summary>
        /// <returns>A list of appointments</returns>
        /// <example>
        /// GET: api/AppointmentData/GetAppointments
        /// </example>
        [ResponseType(typeof(IEnumerable<AppointmentDto>))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAppointments()
        {
            List<Appointment> Appointments = db.Appointments.ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto> { };

            foreach (var Appointment in Appointments)
            {
                AppointmentDto NewAppointment = new AppointmentDto
                {
                    AppId = Appointment.AppId,
                    AppMethod = Appointment.AppMethod,
                    AppPurpose = Appointment.AppPurpose,
                    AppDate = Appointment.AppDate,
                    DAppDate = Appointment.AppDate.ToString("MMM d yyyy"),
                    AppTime = Appointment.AppTime,
                    StaffId = Appointment.StaffId,
                    UserId = Appointment.UserId,
                    FirstName = Appointment.User.FirstName,
                    LastName = Appointment.User.LastName,
                    StaffFirstName = Appointment.StaffInfo.StaffFirstName,
                    StaffLastName = Appointment.StaffInfo.StaffLastName,
                    DepartmentName = Appointment.StaffInfo.DepartmentName
                    
                };
                AppointmentDtos.Add(NewAppointment);
            }

            //Passes data as 200 status code
            return Ok(AppointmentDtos);

        }


        /// <summary>
        /// Gets a list of appointments associated with a logged in user
        /// </summary>
        /// <param name="id">The logged in UserId</param>
        /// <returns>A list of appointments associated with a logged in user</returns>
        /// <example>
        /// GET: api/InvoiceData/GetAppointmentsForUser/abcdef-12345-ghijkl
        /// </example>

        [Authorize(Roles = "User")]
        public IHttpActionResult GetAppointmentsForUser(string id)
        {
            IEnumerable<Appointment> Appointments = db.Appointments.Where(i => i.UserId == id).ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto> { };

            foreach (var Appointment in Appointments)
            {
                AppointmentDto NewAppointment = new AppointmentDto
                {
                    AppId = Appointment.AppId,
                    AppMethod = Appointment.AppMethod,
                    AppPurpose = Appointment.AppPurpose,
                    AppDate = Appointment.AppDate,
                    DAppDate = Appointment.AppDate.ToString("MMM d yyyy"),
                    AppTime = Appointment.AppTime,
                    StaffId = Appointment.StaffId,
                    StaffFirstName = Appointment.StaffInfo.StaffFirstName,
                    StaffLastName = Appointment.StaffInfo.StaffLastName,
                    UserId = Appointment.UserId
                };
                AppointmentDtos.Add(NewAppointment);
            }

            //Passes data as 200 status code
            return Ok(AppointmentDtos);

        }


        /// <summary>
        /// Gets a list of staff (doctors) that can be selected to book an appointment with
        /// </summary>
        /// <param name="id">The input staff id</param>
        /// <returns>A list of staff</returns>
        /// <example>
        /// GET: api/AppointmentData/GetStaffForAppointment
        /// </example>
        [ResponseType(typeof(IEnumerable<StaffInfoDto>))]
        public IHttpActionResult GetStaffForAppointment(int id)
        {
            //Change later to only get staff based off of deparments
            List<StaffInfo> StaffInfos = db.StaffInfoes.ToList();
            List<StaffInfoDto> StaffInfoDtos = new List<StaffInfoDto> { };

            foreach (var Staff in StaffInfos)
            {
                StaffInfoDto NewStaff = new StaffInfoDto
                {
                    StaffID = Staff.StaffID,
                    StaffFirstName = Staff.StaffFirstName,
                    StaffLastName = Staff.StaffLastName,
                    DepartmentId = Staff.DepartmentId
                };
                StaffInfoDtos.Add(NewStaff);
            }
            return Ok(StaffInfoDtos);
        }


        /// <summary>
        /// Get a single appointment in database with a 200 status code. Returns 404 status code if appointment is not found
        /// </summary>
        /// <param name="id">The appointment ID</param>
        /// <returns>Information about the appointment (title, description, cost, etc.) </returns>
        /// <example>
        /// GET: api/AppointmentData/FindAppointment/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AppointmentDto))]
        [Authorize(Roles = "Admin,User")]
        public IHttpActionResult FindAppointment(int id)
        {
            //Finds data
            Appointment Appointment = db.Appointments.Find(id);
            //If data is not found, returns 404 status code
            if (Appointment == null)
            {
                return NotFound();
            }

            AppointmentDto AppointmentDto = new AppointmentDto
            {
                AppId = Appointment.AppId,
                AppMethod = Appointment.AppMethod,
                AppPurpose = Appointment.AppPurpose,
                AppDate = Appointment.AppDate,
                DAppDate = Appointment.AppDate.ToString("MMM d yyyy"),
                AppTime = Appointment.AppTime,
                StaffId = Appointment.StaffId,
                UserId = Appointment.UserId,
                FirstName = Appointment.User.FirstName,
                LastName = Appointment.User.LastName,
                Email = Appointment.User.Email,
                StaffFirstName = Appointment.StaffInfo.StaffFirstName,
                StaffLastName = Appointment.StaffInfo.StaffLastName,
                DepartmentName = Appointment.StaffInfo.DepartmentName
            };

            //Passes data as 200 status code
            return Ok(AppointmentDto);
        }


        /// <summary>
        /// Finds staff that is associated with particular appointment
        /// </summary>
        /// <param name="id">The appointment id</param>
        /// <returns>Information about the staff member associated with appointment</returns>
        /// <example>
        /// GET: api/AppointmentData/FindStaffForAppointment/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AppointmentDto))]
        public IHttpActionResult FindStaffForAppointment(int id)
        {
            //Find data
            StaffInfo Staff = db.StaffInfoes.Where(s => s.Appointment.Any(a => a.AppId == id)).FirstOrDefault();
            if (Staff == null)
            { 
                return NotFound();
            }

            StaffInfoDto StaffInfoDto = new StaffInfoDto
            {
                StaffID = Staff.StaffID,
                StaffFirstName = Staff.StaffFirstName,
                StaffLastName = Staff.StaffLastName
            };
            return Ok(StaffInfoDto);

        }

        /// <summary>
        /// Finds user that is associated with particular appointment
        /// </summary>
        /// <param name="id">The appointment id</param>
        /// <returns>Information about the user associated with appointment</returns>
        /// <example>
        /// GET: api/AppointmentData/FindUserForAppointment/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AppointmentDto))]
        public IHttpActionResult FindUserForAppointment(int id)
        {
            //Find data
            ApplicationUser User = db.Users.Where(u => u.Appointments.Any(a => a.AppId == id)).FirstOrDefault();
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
        /// Update an appointment in the database given information about the appointment
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <param name="appointment">An Appointment Object. Received as POST data</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/AppointmentData/UpdateAppointment/5
        /// FORM DATA: Appointment JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public IHttpActionResult UpdateAppointment(int id, [FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.AppId)
            {
                return BadRequest();
            }

            db.Entry(appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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
        /// Adds an appointment to the database
        /// </summary>
        /// <param name="appointment">An appointment Object. Sent as Post Form data.</param>
        /// <returns>Status Code 200 if successful, 400 in unsuccessful</returns>
        /// <example>
        /// FORM DATA: Appointment JSON Object
        /// </example>
        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult AddAppointment([FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            appointment.UserId = User.Identity.GetUserId();

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return Ok(appointment.AppId);
        }

        /// <summary>
        /// Deletes appointment in the database
        /// </summary>
        /// <param name="id">The id of the appointment to delete</param>
        /// <returns>200 if successful, 400 in unsuccessful.</returns>
        /// <example>
        /// POST: api/AppointmentData/DeleteAppointment/5
        /// </example> 
        [HttpPost]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppId == id) > 0;
        }
    }
}