Example legacy webforms project to be used alongside below candidate test:

Modernising a legacy application
This part involves taking a legacy application and rebuilding it in a modern stack. 
Getting set up
1.	Install Microsoft SQL Server Management Studio (SSMS) and SQL Express Server (if you don’t already have it)
a.	See https://support.bridgeworksllc.com/hc/en-us/articles/4404927691796-Microsoft-SQL-Express-2022-SSMS-Installation (there are many other articles available on how to do this)
b.	Ensure you can connect to your locally running SQL Express server from SSMS
2.	Install Microsoft Visual Studio (if you don’t already have it)
  a.	https://visualstudio.microsoft.com/vs/community/
  b.	On the Visual studio installer, ensure to install the tools for asp.net and web development, selecting .Net Framework 4.7.2 development tools
3.	Get the legacy web application
  a.	Download/Unzip the attached web application folder “ClientManagementWebforms-master”
4.	Attach database to run locally
  a.	Open Microsoft SQL Server Management Studio and connect to your locally running SQL Server instance
  b.	Attach the .mdf file which is in the web application folder in /DB
  c.	The database should be available to query, take a quick look at the tables and run a query to ensure you can get some data
  OR:
  - In /DB/Scripts there are 2 scripts “CreateDBScript” will create the database and it’s associated objects/stored procs, the “initDynamicallyAddData” script will dynamically create demo records
5.	Changing connection string in the application
  a.	Open the web application in Visual Studio by opening the ClientManagementWebforms.sln file in the root of the application folder
  b.	Open the web.config file and change the connection string to point to your local SQL server instance (don’t worry about extracting this out into env vars etc., it’s just to run locally), it’s currently set up to connect using the windows login, you can change it to a DB login if you prefer.
6.	Run the application
  a.	Run the application in debug in VS, ensure the application runs and you can navigate around it with data being visible in the grids and client form.

The Task - Rebuilding the application
The task is to create a new application using a modern framework and architecture that replicates the functionality of the legacy application, as well as optimising the stored procedures that are used for generating the “dashboard” data.

Timebox and expectations
Estimated time to deliver is around 5-7 hours, it doesn’t have to be a perfect replica and focus should be on functionality and showing your knowledge. You can stop at 7 hours and put together a doc detailing what you’d do next if you run out of time.

Deliverables
-	Backend API (.NET 9): Build a REST API using Dependency Injection. Connect to the database to handle the requirements below.
-	Frontend (React + TypeScript): Bootstrap using Vite or Next.js.
-	Optimization Task: Identify the Stored Procedures used for the legacy Dashboard. Optimize the SQL queries for performance while ensuring the data output remains identical.
-	Clients Page: A grid view displaying all clients with "Open" and "Delete" actions.
-	Client Record: A form to view, edit, and save client details. (Focus on functionality for a few key fields rather than implementing every single field from the legacy app). Feel free to use form management libraries if needed.
-	Journal Functionality: Within the Client Record, include the ability to View, Add, and Update journal entries associated with that client.
-	Lookups: utilise the existing Lookup and LookupValue tables to populate dropdowns in your UI.
-	GitHub: Provide links to repositories for the API and Frontend with a standard README.md on how to run them.

Key Notes:
-	Data Access/Reusing existing code: feel free to reuse existing c# code where/if possible (could you reuse the existing code responsible for DB interaction and running stored procs? Could it be extracted out into a separate project in the solution so it can be reused in other projects?), you can add notes on how it could be improved in the future.
-	Authentication: don’t worry about authentication/authorisation.
-	UI/UX: Use a library like Tailwind/Bootstrap, don’t invest too much time on this, just use standard classes to keep it clean. Focus should be on functionality.
-	Time Management: The estimate is for this to take 5-7 hours. If you run out of time, please stop and write a brief "Next Steps" document explaining what you would have finished.
-	Notes: feel free to add comments highlighting why you’ve done certain things or add a supporting document detailing any of these notes
