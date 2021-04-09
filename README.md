<h1>Lake of the Humber</h1>

MVP Submition - 09/04/2020

1. Choo 
2. Rohit
3. Asia
4. Daniel
5. Praveen
<h4>CRUD 1: Information Section</h4>

Controllers -> InfoSectionsController.cs, InfoSectionsDataController.cs

Models -> InfoSection.cs, WellWish.cs

Views -> InfoSections/


<h4>CRUD 2: Well Wishes</h4>

Controllers -> WellWishesController.cs, WellWishesDataController.cs

Models -> WellWish.cs

Views -> WellWishes/



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
