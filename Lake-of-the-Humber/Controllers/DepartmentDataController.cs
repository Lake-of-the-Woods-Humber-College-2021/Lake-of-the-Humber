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
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieve all Departments information
        /// </summary>
        /// <returns>A list of Departments with their information on Name, PhoneNumber, and address</returns>
        ///<example> GET: api/DepartmentData/GetDepartments </example>
        [ResponseType(typeof(IEnumerable<DepartmentDto>))]
        public IEnumerable<DepartmentDto> GetDepartments()
        {
            List<Department> Departments = db.Departments.ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Department in Departments)
            {
                DepartmentDto NewDepartment = new DepartmentDto
                {
                    DepartmentId = Department.DepartmentId,
                    DepartmentName = Department.DepartmentName,
                    DepartmentAddress = Department.DepartmentAddress,
                    DepartmentPhone = Department.DepartmentPhone,

                };
                DepartmentDtos.Add(NewDepartment);
            }

            return (DepartmentDtos);
        }


        /// <summary>
        /// Finds a particular Department in the database with a 200 status code. If the Department is not found, return 404.
        /// </summary>
        /// <param name="id">The Department id</param>
        /// <returns>Information about the particular Department includes Name, PhoneNumber and Address  </returns>
        // <example>
        // GET: api/DepartmentData/FindDepartment/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult FindDepartment(int id)
        {
            //Find a Department Data
            Department Department = db.Departments.Find(id);
            //If not found, return 404 status code
            if (Department == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            DepartmentDto DepartmentDto = new DepartmentDto
            {
                DepartmentId = Department.DepartmentId,
                DepartmentName = Department.DepartmentName,
                DepartmentAddress = Department.DepartmentAddress,
                DepartmentPhone = Department.DepartmentPhone,
            };

            //pass along data as 200 status code OK response
            return Ok(DepartmentDto);
        }

        /// <summary>
        /// Updates a Department information after any changes apply.
        /// </summary>
        /// <param name="id">The Department id</param>
        /// <param name="Department">A Department object. Received as POST data.</param>
        /// <returns>Updated Department infromation on Database</returns>
        /// <example>
        /// POST: api/DepartmentData/UpdateDepartment/5
        /// FORM DATA: Department JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDepartment(int id, [FromBody] Department Department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Department.DepartmentId)
            {
                return BadRequest();
            }

            db.Entry(Department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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
        /// Add a Department to the database after receive the GET form information
        /// </summary>
        /// <param name="Department">A Department object. Sent as POST form data.</param>
        /// <returns>If request is OK indicated, it is working! if failed, it will return error message of 400 status code</returns>
        /// <example>
        /// POST: api/DepartmentData/AddDepartment
        ///  FORM DATA: Department JSON Object
        /// </example>
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult AddDepartment([FromBody] Department Department)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(Department);
            db.SaveChanges();

            return Ok(Department.DepartmentId);
        }


        /// <summary>
        /// Receive Get information from client-side and deletes a Department in the database
        /// </summary>
        /// <param name="id">The id of the department to delete.</param>
        /// <returns>If failed to delete , it will show error page. If succeed , it will show save the changes and confimed request of 200 status code </returns>
        /// <example>
        /// POST: api/DepartmentData/DeleteDepartment/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
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
        /// Finds a department in the system. Internal use only.
        /// </summary>
        /// <param name="id">The department id</param>
        /// <returns>TRUE if the department exists, false otherwise.</returns>
        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }
    }
}
