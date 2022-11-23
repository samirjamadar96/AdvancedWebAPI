using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AdvancedWebAPI.Common;
using AdvancedWebAPI.Extensions;
using AdvancedWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdvancedWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly AdventureWorks2019Context _context;

        public PersonController(AdventureWorks2019Context context)
        {
            _context = context;

            _context.Database.EnsureCreated();
        }

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

        [HttpGet("GetPersons")]
        public IEnumerable<object> GetPersons([FromQuery] PersonQueryParameters queryParameters)
        {
            IQueryable<Person> person = _context.People;

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

            person = person
                        .Skip(queryParameters.Size * (queryParameters.Page - 1))
                        .Take(queryParameters.Size);

            return person.ToArray();
        }
    }
}

