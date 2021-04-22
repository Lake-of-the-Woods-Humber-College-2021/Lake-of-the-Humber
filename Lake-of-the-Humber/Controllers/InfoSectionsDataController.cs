using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Lake_of_the_Humber.Models;

namespace Lake_of_the_Humber.Controllers
{
    public class InfoSectionsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Gets a list of active sections in the database alongside a status code (200 OK) to display on the homepage.
        /// </summary>
        /// <returns>A list containing maximum of 5 sections and their information.</returns>
        /// <example>
        /// GET : api/InfoSectionsData/GetHomepageInfoSection
        /// </example>
        [ResponseType(typeof(IEnumerable<InfoSectionDto>))]
        public IHttpActionResult GetHomepageInfoSection()
        {
            List<InfoSection> InfoSections = db.InfoSections.Where(section => !section.IsArchive).OrderBy(section => section.PriorityNumber).Take(5).ToList();
            List<InfoSectionDto> InfoSectionsDtos = new List<InfoSectionDto> { };

            foreach (var Section in InfoSections)
            {
                InfoSectionDto NewSection = new InfoSectionDto
                {
                    SectionId = Section.SectionId,
                    SectionTitle = Section.SectionTitle,
                    SectionDescription = Section.SectionDescription,
                    PriorityNumber = Section.PriorityNumber,
                    Link = Section.Link,
                    LinkBtnName = Section.LinkBtnName,
                    SectionImageExt = Section.SectionImageExt
                };

                InfoSectionsDtos.Add(NewSection);
            }
            return Ok(InfoSectionsDtos);
        }

        /// <summary>
        /// Gets a list of all sections in the database alongside a status code (200 OK) to display in admin panel.
        /// </summary>
        /// <returns>A list of all sections and their information.</returns>
        /// <example>
        /// GET : api/InfoSectionsData/GetAllSections
        /// </example>
        [ResponseType(typeof(IEnumerable<InfoSectionDto>))]
        public IHttpActionResult GetAllSections()
        {
            List<InfoSection> InfoSections = db.InfoSections.ToList();
            List<InfoSectionDto> InfoSectionsDtos = new List<InfoSectionDto> { };

            foreach (var Section in InfoSections)
            {
                InfoSectionDto NewSection = new InfoSectionDto
                {
                    SectionId = Section.SectionId,
                    SectionTitle = Section.SectionTitle,
                    SectionDescription = Section.SectionDescription,
                    PriorityNumber = Section.PriorityNumber,
                    Link = Section.Link,
                    LinkBtnName = Section.LinkBtnName,
                    SectionImageExt = Section.SectionImageExt,
                    IsArchive = Section.IsArchive,
                    CreatorId = Section.CreatorId
                };

                InfoSectionsDtos.Add(NewSection);
            }
            return Ok(InfoSectionsDtos);
        }

        /// <summary>
        /// Finds a particular section in the database with a 200 status code. If the section is not found, return 404.
        /// </summary>
        /// <param name="id">The section id</param>
        /// <returns>Information about the section</returns>
        /// <example>
        /// GET: api/InfoSectionsData/GetSection/5
        /// </example>
        [ResponseType(typeof(InfoSectionDto))]
        public IHttpActionResult GetSection(int id)
        {
            InfoSection InfoSection = db.InfoSections.Find(id);
            if(InfoSection == null)
            {
                return NotFound();
            }
            InfoSectionDto InfoSectionDto = new InfoSectionDto
            {
                SectionId = InfoSection.SectionId,
                SectionTitle = InfoSection.SectionTitle,
                SectionDescription = InfoSection.SectionDescription,
                PriorityNumber = InfoSection.PriorityNumber,
                Link = InfoSection.Link,
                LinkBtnName = InfoSection.LinkBtnName,
                SectionImageExt = InfoSection.SectionImageExt,
                IsArchive = InfoSection.IsArchive,
                CreatorId = InfoSection.CreatorId
            };
            return Ok(InfoSectionDto);
        }

        /// <summary>
        /// Deletes a section in the database
        /// </summary>
        /// <param name="id">The id of the section to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/InfoSectionsData/DeleteSection/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteSection(int id)
        {
            InfoSection InfoSection = db.InfoSections.Find(id);

            if (InfoSection == null)
            {
                return NotFound();
            }
            db.InfoSections.Remove(InfoSection);

            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Adds new section to database.
        /// </summary>
        /// <param name="Section">A Section object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/InfoSectionsData/AddSection
        /// FORM DATA: Info Section JSON Object
        /// </example>
        [ResponseType(typeof(int))]
        [HttpPost]
        public IHttpActionResult AddSection([FromBody] InfoSectionDto Section)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InfoSection InfoSection = new InfoSection()
            {
                SectionTitle = Section.SectionTitle,
                SectionDescription = Section.SectionDescription,
                PriorityNumber = Section.PriorityNumber,
                Link = Section.Link,
                LinkBtnName = Section.LinkBtnName,
                SectionImageExt = Section.SectionImageExt,
                IsArchive = Section.IsArchive,
                CreatorId = Section.CreatorId
            };

            db.InfoSections.Add(InfoSection);
            db.SaveChanges();
            return Ok(InfoSection.SectionId);
        }

        /// <summary>
        /// Updates a Section in the database given information about the Section.
        /// </summary>
        /// <param name="id">The Section id</param>
        /// <param name="Section">A Section object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/InfoSectionsData/UpdateSection/5
        /// FORM DATA: Info Section JSON Object
        /// </example>
        [ResponseType(typeof(InfoSectionDto))]
        [HttpPost]
        public IHttpActionResult UpdateSection(int id, [FromBody] InfoSectionDto Section)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != Section.SectionId)
            {
                return BadRequest();
            }
            InfoSection InfoSection = new InfoSection()
            {
                SectionId = Section.SectionId,
                SectionTitle = Section.SectionTitle,
                SectionDescription = Section.SectionDescription,
                PriorityNumber = Section.PriorityNumber,
                Link = Section.Link,
                LinkBtnName = Section.LinkBtnName,
                SectionImageExt = Section.SectionImageExt,
                IsArchive = Section.IsArchive,
                CreatorId = Section.CreatorId
            };

            db.Entry(InfoSection).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InfoSectionExists(Section.SectionId))
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InfoSectionExists(int id)
        {
            return db.InfoSections.Count(e => e.SectionId == id) > 0;
        }
    }
}