using System;
namespace AdvancedWebAPI.Common
{
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
}

