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
 
 Please refer to the _PageQueryParameters.cs_ class
 
 ```
 public class PaginationQueryParameters
	{
		private int _maxPageSize = 100; // Max size of the page
		public int _size = 50; // default size of the page

		public int Size {
			get { return _size; } // returning the default size on request of get
			set
			{
				_size = Math.Min(_maxPageSize, value); } // comparing the size provided and passing the min value of _maxSize and size value provided
			}

		public int Page { get ; set; } = 1; // Page value initialized to 1
	}
 ```
To use this pagination configuration in action, please refer to the _PersonController.cs_ class.

```
[HttpGet("GetAllPersons")]
        public IEnumerable<object> Get([FromQuery] PaginationQueryParameters queryParameters)
        {
            IQueryable<Person> person = _context.People;


            /*
             * ** Pagination **
             * 1. You need to skip the records first as per page number and page size, hence 'skip' is used
             * 2. Now, You need to consider the number of records to send. For the value of size, please refer to
             *    'PaginationQueryParameters' class wherein the value of Size is decided based on the 
             *    input provided and our max threshold
             */
            var data = person
                        .Skip(queryParameters.Size * (queryParameters.Page - 1))
                        .Take(queryParameters.Size);

            return data.ToArray();
        }
```









