# Lake-of-the-Humber
Northern Ontario Hospital Website Redesign

# Lake of the Humber: MVP Submission - 09/04/2020

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
#### What's Next
- Authentication (based on user roles)
- Image Upload on information section
- showing information section on homepage

#### Files

Controllers -> InfoSectionsController.cs, InfoSectionsDataController.cs

Models -> InfoSection.cs, WellWish.cs

Views -> InfoSections/

---

### CRUD 2: Well Wishes Section (Praveen)
#### Description
- A user can send(create), read, update, delete wellwishes
#### What's Next
- Authentication (based on user roles)
- Email Notification for user once well wish is delivered.

#### Files

Controllers -> WellWishesController.cs, WellWishesDataController.cs

Models -> WellWish.cs

Views -> WellWishes/

---

### CRUD 3: Gift Shop, Product Section (Rohit)

#### Description
-A list of products that are sold in the Gift Shop. CRUD on products.

#### What's Next
-Image Upload, so users can see the images of the products.

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
- A user can view a list of appointments that have been made
- A user can create an appointment by selecting a method, writing out a purpose, selecting a date, selecting a time, selecting a Staff(Doctor) ID, and selecting a User(Patient) ID
- A user can view details for a single appointment (method, purpose, date, time, doctor, patient info)
- A user can edit all components of an appointment, with the execption of the user(patient) ID (a new appointment can be made instead)
- A user can delete an appointment  

#### What's Next
- Displaying Staff(Doctor) Details when creating an appointment (e.g Name, Department, etc.)
- Displaying Staff(Doctor) and User(Patient) details on Details view
- Restricting functionality based off of user logged in
- Automatically including logged in User's ID to be set when creating an appointment

#### Files
Controllers -> AppointmentController.cs, AppointmentDataController.cs

Models -> Appointment.cs, ViewModels/(ListAppointment.cs, ShowAppointment.cs, UpdateAppointment.cs)

Views -> Appointment/(List.cshtml, Details.cshtml, Create.cshtml, Edit.cshtml, DeleteConfirm.cshtml)

---

### CRUD 6: Invoices (Daniel)
#### Description
- A user can view a list of invoices that exist
- A user can create an invoice by writing out a title, description, cost and userID (date and IsPaid status are default)
- A user can view details for a single invoice (title, description, date, payment status, userID)
- A user can edit the title, description, cost, and payment status of an invoice
- A user can delete an invoice  

#### What's Next
- Displaying User details on Details
- Restricting functionality based off of user logged in
- Automatically including logged in User's ID to be set when creating an invoice
- Fix invoice date to stay as the date it was created

#### Files
Controllers -> InvoiceController.cs, InvoiceDataController.cs

Models -> Invoice.cs, ViewModels/(ShowInvoice.cs, UpdateInvoice.cs)

Views -> Invoice/(List.cshtml, Details.cshtml, Create.cshtml, Edit.cshtml, DeleteConfirm.cshtml)

---
### CRUD 7: Department (Choo)
#### Description
- A list of department will be displayed when user click on the "department" link on the website. 
- Public user allowed to view and read the list.
- Admin user will be able to do CRUD oepration for staff Information.

#### What's Next
- Add search functionality to filter the list based on user's selection.

#### Files
Controllers -> DeaprtmentController.cs, DepartmentDataController.cs

Models -> Department.cs, ViewModels/(ShowDepartment.cs, UpdateDepartmentt.cs)

Views -> Department/(List.cshtml, Details.cshtml, Create.cshtml, Edit.cshtml, DeleteConfirm.cshtml)

---
### CRUD 8: Staff Infromation (Choo)
#### Description
-  A list of staff information will be displayed when user click on the "staff Information" link on the website. 
-  Public user allowed to view and do basic search functionality
-  Admin user will be able to do CRUD oepration for staff Information. 

#### What's Next
- Add images , pagination 
- when user clicked on the image, it will show more information bout the doctor (like image Card)
- Add search functionality to filter the list based on user's selection.

#### Files
Controllers -> StaffInfoController.cs, StaffInfoDataController.cs

Models -> StaffInfo.cs, ViewModels/(ShowStafft.cs, UpdateStaff.cs)

Views -> StaffInfo/(List.cshtml, Details.cshtml, Create.cshtml, Edit.cshtml, DeleteConfirm.cshtml)

---

### Asia Levesque Gault
- [X] Feature1: FAQ
- [ ] Feature2: Volunteer

The purpose of the FAQ feature is to allow the websites visitors to see freequently asked questions. These Question's CRUD (Create, Read, Update, Delete) operations are controled by the website's Administrators. They will be able to login with Administrator credentials and have their own CMS(Content Management System) to easily add an FAQ, Publish an FAQ, Edit an FAQ and delete an FAQ. Vistitors who are not Administrators will be able to view all FAQs and view the Details of an FAQ.

The purpose of the volunteer feature is to allow the webistes visitors to login using their credentials and view a list of all volunteer positions currently available/made public. They can then click on a volunteer position and view its respective details and apply for it using a form. The Administrator will be able to create CRUD operations of the volunteer positions. They will also be able to choose weather the FAQ is publically available. The Administrators will be able to view applicants details. I will contuinue to work on this features CRUD opperations. 

As of 4:35pm MST I have completed the List, Delete, Create, Update and Detail aspects of the FAQ feature. At this point I am very proud of what I have accomplished. This is already furthur than I had gotten with the Passion Project and The only help I needed was fixed by Prof. Bittle, it was "The Remote Certificate is Invalid". Since then I debugged many issues and I took each on at a time and in small steps. One bugg I got stuck on was I created a viewmodel but forgot to run a migration. I commented everything out deleted the view model. started over and it worked perfectly. All this debugging really has made me understand this a lot more than previous projects. Frankly Debugging was initially frustrating but once you see the # of errors decrease it was fun. This turned into a puddle or a waterslide to see how information flows and talks. I am excited to keep working on this. I am intimidated by github as i do not want to ovewrite anyones work. we are all working for our own branches mine for FAQ is loh_FAQ. I will continue to make lists of steps and flow charts to not make the project explode 🤯🌋💣.

The View of the list B4 deleteing

![List_B4_delete](https://github.com/Lake-of-the-Woods-Humber-College-2021/Lake-of-the-Humber/blob/loh_FAQ/faq_imgs/Listb4Delete.PNG)

The View of the Detail uppon clicking on the FAQ

![Detail_B4_delete](https://github.com/Lake-of-the-Woods-Humber-College-2021/Lake-of-the-Humber/blob/loh_FAQ/faq_imgs/CLicking%20list%20go%20to%20detail.PNG)

The View of the Delete Confirmation page uppon clicking on the Delete button

![Delete_B4_delete](https://github.com/Lake-of-the-Woods-Humber-College-2021/Lake-of-the-Humber/blob/loh_FAQ/faq_imgs/Deletepage.PNG)

The View of the Updated list after deleting

![List_Aft_delete](https://github.com/Lake-of-the-Woods-Humber-College-2021/Lake-of-the-Humber/blob/loh_FAQ/faq_imgs/List%20After%20Delete.PNG)

The View of the DB after deleting

![delete](https://github.com/Lake-of-the-Woods-Humber-College-2021/Lake-of-the-Humber/blob/loh_FAQ/faq_imgs/DbReflectdelet.PNG)

