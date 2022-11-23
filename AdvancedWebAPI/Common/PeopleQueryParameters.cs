using System;
namespace AdvancedWebAPI.Common
{
	public class PersonQueryParameters : PaginationQueryParameters
	{
		public string? NameContains { get; set; }

		public string? PersonType { get; set; }

        public string? SortBy { get; set; }

		public string _sortOrder = "asc";

		public string SortOrder {
			get { return _sortOrder; }
			set {
				if (value == "asc" || value == "desc")
				{
					_sortOrder = value;
				}
			} }
	}
}
