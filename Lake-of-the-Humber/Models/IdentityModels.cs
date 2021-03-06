using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lake_of_the_Humber.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<InfoSection> InfoSections { get; set; }
        public ICollection<WellWish> WellWishes { get; set; }
        public ICollection<StaffInfo> StaffInfoes { get; set; }
        public ICollection<Department> Departments { get; set; }

        //A user can have multiple appointments
        public ICollection<Appointment> Appointments { get; set; }
        //A user can have multiple invoices
        public ICollection<Invoice> Invoice { get; set; }

        //A user can have multiple faqs
        public ICollection<Faq> Faqs { get; set; }

        //A user can have multiple faqs
        public ICollection<Volunteer> Volunteers { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("lohDBConnection", throwIfV1Schema: false)
        {
        }

        //Instruction to set the models as tables in our database.
        /// <summary>
        /// This is the model that is linked to the associated users of the application
        /// </summary>
        public IEnumerable ApplicationUsers { get; internal set; }
        /// <summary>
        /// This is the model that is linked to the Information Section of Homepage
        /// </summary>
        public DbSet<InfoSection> InfoSections { get; set; }


        /// <summary>
        /// This is the model that is linked to the Wellwishes
        /// </summary>
        public DbSet<WellWish> WellWishes { get; set; }

        /// <summary>
        /// This is the model that is linked to Staffs
        /// </summary>
        public DbSet<StaffInfo> StaffInfoes { get; set; }


        /// <summary>
        /// This is the model that is linked to Departments
        /// </summary>
        public DbSet<Department> Departments { get; set; }

        /// <summary>
        /// This is the model that is linked to the Products
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// This is the model that is linked to the Orders
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// This is the model that is linked to Appointments
        /// </summary>
        public DbSet<Appointment> Appointments { get; set; }

        /// <summary>
        /// This is the model that is linked to Invoices
        /// </summary>
        public DbSet<Invoice> Invoices { get; set; }

        /// <summary>
        /// This is the model that is linked to the Wellwishes
        /// </summary>
        public DbSet<Faq> Faqs { get; set; }

        /// <summary>
        /// This is the model that is linked to the Wellwishes
        /// </summary>
        public DbSet<Volunteer> Volunteers { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}