﻿                                                       using System;
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
    public class WellWishesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets a list of wellwishes in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list containing wellwishes and their information.</returns>
        /// <example>
        /// GET : api/WellWishesData/GetAllWellWishes
        /// </example>
        [ResponseType(typeof(IEnumerable<InfoSectionDto>))]
        public IHttpActionResult GetAllWellWishes()
        {
            List<WellWish> WellWishes = db.WellWishes.ToList();
            List<WellWishDto> WellWishesDtos = new List<WellWishDto> { };

            foreach (var Wish in WellWishes)
            {
                WellWishDto NewWish = new WellWishDto
                {
                    WishId = Wish.WishId,
                    Message = Wish.Message,
                    RoomNumber = Wish.RoomNumber,
                    ReceiverName = Wish.ReceiverName,
                    CreatedDate = Wish.CreatedDate,
                    IsReceived = Wish.IsReceived,
                    // ReceivedDate = Wish.ReceivedDate
                };

                WellWishesDtos.Add(NewWish);
            }
            return Ok(WellWishesDtos);
        }

        /// <summary>
        /// Gets a list of wellwishes created by the logged-in user in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list containing wellwishes and their information.</returns>
        /// <example>
        /// GET : api/WellWishesData/GetUserWellWishes
        /// </example>
        [Route("api/WellWishesData/GetUserWellWishes/{CreatorId}")]
        [ResponseType(typeof(IEnumerable<WellWishDto>))]
        public IHttpActionResult GetUserWellWishes(string CreatorId)
        {
            List<WellWish> WellWishes = db.WellWishes.Where(wish => wish.CreatorId == CreatorId).ToList();
            List<WellWishDto> WellWishesDtos = new List<WellWishDto> { };

            foreach (var Wish in WellWishes)
            {
                WellWishDto NewWish = new WellWishDto
                {
                    WishId = Wish.WishId,
                    Message = Wish.Message,
                    RoomNumber = Wish.RoomNumber,
                    ReceiverName = Wish.ReceiverName,
                    CreatedDate = Wish.CreatedDate,
                    IsReceived = Wish.IsReceived,
                    // ReceivedDate = Wish.ReceivedDate
                };

                WellWishesDtos.Add(NewWish);
            }
            return Ok(WellWishesDtos);
        }

        /// <summary>
        /// Finds a particular wellwish in the database with a 200 status code. If the wellwish is not found, return 404.
        /// </summary>
        /// <param name="id">The wellwish id</param>
        /// <returns>Information about the wellwish</returns>
        /// <example>
        /// GET: api/WellWishesData/GetWish/5
        /// </example>
        [ResponseType(typeof(WellWishDto))]
        public IHttpActionResult GetWish(int id)
        {
            WellWish Wish = db.WellWishes.Find(id);
            if (Wish == null)
            {
                return NotFound();
            }
            WellWishDto WellWishDto = new WellWishDto
            {
                WishId = Wish.WishId,
                Message = Wish.Message,
                RoomNumber = Wish.RoomNumber,
                ReceiverName = Wish.ReceiverName,
                CreatedDate = Wish.CreatedDate,
                IsReceived = Wish.IsReceived,
                ReceivedDate = Wish.ReceivedDate,
                CreatorId = Wish.CreatorId
            };
            return Ok(WellWishDto);
        }

        /// <summary>
        /// Deletes a wellwish in the database
        /// </summary>
        /// <param name="id">The id of the wellwish to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/WellWishesData/DeleteWish/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteWish(int id)
        {
            WellWish Wish = db.WellWishes.Find(id);

            if (Wish == null)
            {
                return NotFound();
            }
            db.WellWishes.Remove(Wish);

            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Adds wellwish to database.
        /// </summary>
        /// <param name="Wish">A Wish object. Sent as POST form data.</param>
        /// <returns>if successful created Id with status code 200 . 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/WellWishesData/AddWish
        /// FORM DATA: Well Wish JSON Object
        /// </example>
        [ResponseType(typeof(int))]
        [HttpPost]
        public IHttpActionResult AddWish([FromBody] WellWishDto Wish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WellWish NewWish = new WellWish()
            {
                Message = Wish.Message,
                RoomNumber = Wish.RoomNumber,
                ReceiverName = Wish.ReceiverName,
                CreatedDate = DateTime.Now,
                IsReceived = false,
                CreatorId = Wish.CreatorId
            };

            db.WellWishes.Add(NewWish);
            db.SaveChanges();
            return Ok(NewWish.WishId);
        }

        /// <summary>
        /// Updates a Wish in the database given information about the Wish.
        /// </summary>
        /// <param name="id">The Wish id</param>
        /// <param name="Wish">A Wish object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/WellWishesData/UpdateWish/5
        /// FORM DATA: Well Wish JSON Object
        /// </example>
        [ResponseType(typeof(InfoSectionDto))]
        [HttpPost]
        public IHttpActionResult UpdateWish(int id, [FromBody] WellWishDto Wish)
        {
            Debug.WriteLine(Wish.ReceiverName);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(id != Wish.WishId)
            {
                return BadRequest();
            }

            WellWish WellWish = new WellWish()
            {
                WishId = id,
                Message = Wish.Message,
                RoomNumber = Wish.RoomNumber,
                ReceiverName = Wish.ReceiverName,
                CreatedDate = Wish.CreatedDate,
                CreatorId = Wish.CreatorId,
                IsReceived = true,
                ReceivedDate = DateTime.Now
            };

            db.Entry(WellWish).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
                return Ok(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WellWishExists(id))
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

        private bool WellWishExists(int id)
        {
            return db.WellWishes.Count(e => e.WishId == id) > 0;
        }
    }
}