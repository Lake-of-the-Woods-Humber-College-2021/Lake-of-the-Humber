# Lake-of-the-Humber
Northern Ontario Hospital Website Redesign

# Lake of the Humber

## Team Members
1. Choo 
2. Rohit
3. Asia
4. Daniel
5. Praveen

---

### How to run the project
1. Clone the repository
2. Verify the database name in Web.config file
the connectionStrings should look similar this, if you encounter any errors change the name & AttachDbFilename
```
<connectionStrings>
	  <add name="AnyStats" connectionString="Data Source=(localdb)\MSSQLLocalDB; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|AnyStats03- 03-2021.mdf"providerName="System.Data.SqlClient" />
</connectionStrings> 
```
3. Clean, Rebuild project to avoid roslyn error.
4. Verify the App_Data folder is created in the file explorer where the solution exists.
5. In Tools > Nuget Package Manager > Package Manager Console enter following commands in order
  
  &nbsp;&nbsp;&nbsp;&nbsp;enable-migrations
  
  &nbsp;&nbsp;&nbsp;&nbsp;add-migration {migration_name}
  
  &nbsp;&nbsp;&nbsp;&nbsp;update-database

### References
Varsity:- https://github.com/christinebittle/varsity_mvp


## "Features"

### CRUD 1: Information Section (Praveen)

#### Description
- A user can create, delete, update, read the information section
- Authentication based on user roles
- MVP Feedback Incoperated: Linked Departments to infosections
- Managed GitHub Repo and Helped everyone to fix their issues

#### What's Next
- showing information section on homepage

#### Files

Controllers -> InfoSectionsController.cs, InfoSectionsDataController.cs

Models -> InfoSection.cs, ViewModels/InfoSections/

Views -> InfoSections/

---

### CRUD 2: Well Wishes Section (Praveen)

#### Description
- A user can send(create), read, update, delete wellwishes
- Authentication based on user roles
- listing only the wellwishes created by the user for loggedin user & all wellwishes for admin.
- Pagination

#### What's Next
- Email Notification for user once well wish is delivered.

#### Files

Controllers -> WellWishesController.cs, WellWishesDataController.cs

Models ->  WellWish.cs, ViewModels/WellWishes/ 

Views -> WellWishes/

---

### CRUD 3: Gift Shop, Product Section (Rohit)

#### Description
-A list of products that are sold in the Gift Shop. CRUD on products.

New Additions
-Image upload on products and admin authorization for products. Only admin are allowed to preform CRUD on products.

#### Files

Controllers -> ProductController.cs, ProductDataController.cs

Models -> Product.cs

Views -> Product/

---

### CRUD 4: Gift Shop, Order Section (Rohit)
#### Description
-A list of orders made by people. Orders can see who made it, the order message and all the product they had purchased.
#### What's Next
-Adding the total of the products and maybe a payment api.

#### Files

Controllers -> OrderController.cs, OrderDataController.cs

Models -> Order.cs

Views -> Order/

---

### CRUD 5: Appointments (Daniel)
#### Description
- Functionality restricted, only logged in users can view appointments (user or admin)
- An admin can view a full list of appointments that exist
- A user can view a list of appointments that belong to them
- An admin/user can create an appointment by selecting a method (from dropdown), writing out a purpose, selecting a date, selecting a time (from dropdown), selecting a Staff (from dropdown)
    - The appointment will automatically apply the logged in user's userId as a value
- An admin/user can view details for a single appointment (method, purpose, date, time, doctor, patient info [for admin only])
- An admin/user can edit all components of an appointment as long as the appointment has not passed, with the execption of the user(patient) (a new appointment can be made instead)
    - A user can only edit appointments (that have not passed) associated to them. An admin can make changes to any appointment.
- A /adminuser can "cancel" (delete) an appointment  , as long as the appointment has not passed

#### Files
Controllers -> AppointmentController.cs, AppointmentDataController.cs

Models -> Appointment.cs, ViewModels/(ListAppointment.cs, ShowAppointment.cs, UpdateAppointment.cs)

Views -> Appointment/(List.cshtml, Details.cshtml, Create.cshtml, Edit.cshtml, DeleteConfirm.cshtml)

---

### CRUD 6: Invoices (Daniel)
#### Description
- Functionality restricted, only logged in users can view invoices (user or admin)
- An admin can view a full list of invoices that exist
- A user can view a list of invoices that belong to them
- A admin can create an invoice by writing out a title, description, cost and selecting a user from a dropdown list (date &value automatically applied as date created).
- An admin/user can view details for a single invoice (title, description, date, payment status, user info [admin only])
    - A user can only view details for invoices that belong to them, an admin can view details for an invoice
- An admin can edit the title, description, cost, and payment status of an invoice
- An admin can delete an invoice  
- For logged in Users, if the invoice has not been paid, an link will appear allowing the User to "Make a Payment" (not a functioning feature). If it has been paid, that option will not appear


#### Files
Controllers -> InvoiceController.cs, InvoiceDataController.cs

Models -> Invoice.cs, ViewModels/(ListInvoice.cs, ShowInvoice.cs, UpdateInvoice.cs)

Views -> Invoice/(List.cshtml, Details.cshtml, Create.cshtml, Edit.cshtml, DeleteConfirm.cshtml)

---

### Additional Contribution: (Daniel)
- Homepage & Shared Layout development (from team mockup, includes responsive design)
 
---
### CRUD 7: Department (Choo)
#### Description
- A list of department will be displayed when user click on the "department" link on the website. 
- Public user allowed to view and read the list but not able to do Add, Update and Delete operation.
- Admin user will be able to do CRUD oepration for staff Information.

#### What's Next
- Add search functionality to filter the list based on user's selection.
- Design views based on whether user is admin or non-admin.

#### Files
Controllers -> DeaprtmentController.cs, DepartmentDataController.cs

Models -> Department.cs, ViewModels/(ShowDepartment.cs, UpdateDepartmentt.cs)

Views -> Department/(List.cshtml, Details.cshtml, Create.cshtml, Edit.cshtml, DeleteConfirm.cshtml)

---
### CRUD 8: Staff Infromation (Choo)
#### Description
-  A list of staff information will be displayed when user click on the "staff Information" link on the website. 
-  Public user allowed to view and read the list but not able to do Add, Update and Delete operation.
-  Public user able to read more details about staff's working department after click on " Name Link"[Details] on image card
-  Admin user will be able to do CRUD oepration for staff Information. 


#### What's Next
- when user clicked on the image, it will show more information bout the doctor (like image Card)
- Add search functionality to filter the list based on user's selection.
- Design views based on whether user is admin or non-admin.

#### Files
Controllers -> StaffInfoController.cs, StaffInfoDataController.cs

Models -> StaffInfo.cs, ViewModels/(ShowStafft.cs, UpdateStaff.cs)

Views -> StaffInfo/(List.cshtml, Details.cshtml, Create.cshtml, Edit.cshtml, DeleteConfirm.cshtml)

---
### Special Thanks to 

- Praveen who always help the team members to solve technical issues on our project and guide us on handling Git issues.
- Daniel who help me in debug and design the project's homepage.
- It is a nice collaboration with the team members to help each other on debugging and answer questions.

### CRUD 9: FAQ (Asia)
#### Description
-  The purpose of the FAQ feature is to allow the websites visitors to see freequently asked questions. These Question's CRUD (Create, Read, Update, Delete) operations are controled by the website's Administrators. They will be able to login with Administrator credentials and have their own CMS(Content Management System) to easily add an FAQ, Publish an FAQ, Edit an FAQ and delete an FAQ. Vistitors who are not Administrators will be able to view all FAQs and view the Details of an FAQ.
-  A list of FAQs are found under the information tab of the 
-  Public users are allowed to view and read the list.
-  Public users are able to see more details about FAQs clicking on "Details"
-  Full CRUD functionality with regards to the feature
-  Bootstrap styling was added to make it more asthetically pleasing.
-  Allowing the creators name to be displayed under Details and Edit is important for accuntability.
-  In the "Edit" view if a user is registered they are able to add a photo to the DB.(Has been throwing an error. To me it seems like it refuses to make the data design of FAQ accept NULL values for the File name. I Have tried clicking the box "allow nullable" however it won't save the change).  

#### What's Next
- In the futur user Auth will be added. I understand the process: Get the session cookie from the view controller and pass it off to the API controller. In addition to one function. 

#### Files
Controllers -> FaqController.cs, FaqDataController.cs

Models -> Faq.cs, ViewModels/(ShowFaq.cs, UpdateFaq.cs)

Views -> Faq/(List.cshtml, Details.cshtml, Create.cshtml, Edit.cshtml, DeleteConfirm.cshtml)

### CRUD 10: Volunteer (Asia)
#### Description
-  The purpose of the volunteer feature is to allow the webistes visitors to login using their credentials and view a list of all volunteer positions currently available/made public. They can then click on a volunteer position and view its respective details and apply for it using a form. The Administrator will be able to create CRUD operations of the volunteer positions. They will also be able to choose weather the FAQ is publically available. The Administrators will be able to view applicants details. I will contuinue to work on this features CRUD opperations. 
-  A list of volunteer positions are found under the "careers & volunteer" tab of the 
-  Public users are allowed to view and read the list.
-  Public users are able to see more details about FAQs clicking on "Details"
-  Full CRUD functionality with regards to the feature
-  Bootstrap styling was added to make it more asthetically pleasing.
-  Allowing the creators name to be displayed under Details and Edit is important for accuntability.

#### What's Next
- In the futur user Auth will be added. I understand the process: Get the session cookie from the view controller and pass it off to the API controller. In addition to one function. 

#### Files
Controllers -> VolunteerController.cs, VolunteerDataController.cs

Models -> Volunteer.cs, ViewModels/(ShowVolunteer.cs, UpdateVolunteer.cs)

Views -> Volunteer/(List.cshtml, Details.cshtml, Create.cshtml, Edit.cshtml, DeleteConfirm.cshtml)

### Special Thanks to 
- Praveen: My gitHub Guru.
- Cho: Leadership skills/organizational skills to aspire to have.
- Danny: Offered debugging help.
- Journey: Always down to take a frustrated dischord call and give me a hand.
- Overall the whole team worked efficiently together to tackle any issues that arose.
---




