# AdvancedWebAPI
Demonstrated frequently used mechanisms of Web API - 
 ##### 1. Pagination
 ##### 2. Filtering
 ##### 3. Searcing
 ##### 4. Sorting
 
 ## Pre-requisites
 
 This project uses following components -
  1. SQL Server 2019 Express (using Docker as I am using Mac)
  2. Visual Studio 2022
  3. .Net 6
  4. AdventureWorks2019 database file [link here](https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorks2019.bak)
  5. Update your connection string in Appsettings.json file (key name "ConnectionString") - 
        _Server=localhost;Database=AdventureWorks2019;Trusted_Connection=True;User Id=<username value>;Password=<password value>;TrustServerCertificate=Yes;integrated security=false;_

 ## Helper links
 1. How to install SQL server and created Database in Mac Machine ? - (https://setapp.com/how-to/install-sql-server)
 
 ## Details
 
 #### 1. Pagination
 This is one of the most common mechanisms while you deal with REST API and big size payloads.
 Here are the simple calculations for this -
 First of all, we need to define two things : 1. default page size, 2. max page size value
 This way, you are setting or coniguring the pagination mechansim for your application.
 
 
