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


 
 #### 2. Filtering
	
Filtering means to get the required data/records from the whole set of output based on one or many conditions.
Here LINQ queries are very useful in terms of writing the logic and its readability as well.
	
Please refer to this code snippet from _GetPersons_ method of _PersonController.cs_ class -
	
	Here, out of all the person records we are filtering it with given PersonType value
	
	```
	    /*
             * ** Filter **
             * Filter the person records with its 'personType'
             * possible values from the DB are - [ IN, EM, SP, SC, VC, GC ]
             */
            if (!string.IsNullOrEmpty(queryParameters.PersonType))
            {
                person = person.Where(
                            p => p.PersonType.ToLower().Equals(queryParameters.PersonType.ToLower())
                        );
            }

	```
	
 #### 3. Searching

Searching is kind of filtering the data/records but we need to search for the input conditions with all the records and extract the matching records.
Please refer to this code snippet from _GetPersons_ method of _PersonController.cs_ class -
	
	Here, we are searching for the records which contains the input string in its first name or last name.
	
	```
	    /*
             * ** Search **
             * Search in the 'FirstName' and 'LastName' field that contains the given input string
             */
            if (!string.IsNullOrEmpty(queryParameters.NameContains))
            {
                person = person.Where(
                            p => p.FirstName.ToLower().Contains(queryParameters.NameContains.ToLower()) ||
                                 p.LastName.ToLower().Contains(queryParameters.NameContains.ToLower())
                        );
            }
	```

 #### 4. Sorting

Sorting is something which refers to ordering of the records. The default order for sorting the records is 'ASCENDING' but you can explicitly define the ordering if you don't want the default one (i.e. DESCENDING). 
Also, It needs the column of table to sort by.
Please refer to this code snippet from _GetPersons_ method of _PersonController.cs_ class -
	
	```
	    /*
             * ** Sort **
             * Here we can sort our records with user provided column, provided the column should be present inside table
             * We have first checked whether the column exist or not with case-insensitive approach
             * then we used custom extension method to sort
             */
            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                var defaultLookup = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

                if (typeof(Person).GetProperty(queryParameters.SortBy, defaultLookup) != null)
                {
                    person = person.OrderByCustom(
                                queryParameters.SortBy,
                                queryParameters.SortOrder
                        );
                }
            }

	```
	
	Here, we are sorting for the records with given input value (1. column name and 2. sort by).
	For input, 
	1. Column name (_queryParameters.SortBy_) we need to first check whether the input column name is present in the table (added extra logic to ignore the case of the given string)
	2. Sort order (_queryParameters.SortOrder_) We need to first check whether the input string is valid for the two possible options (i.e. ASC (ascending) or DESC (descending)). Hence, added the extra logic in _PersonQueryParameters.cs_ class.
	
	```
	public string SortOrder {
			get { return _sortOrder; }
			set {
				if (value == "asc" || value == "desc")
				{
					_sortOrder = value;
				}
			} }
	}
	```




