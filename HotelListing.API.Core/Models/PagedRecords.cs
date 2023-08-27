﻿namespace HotelListing.API.Core.Models
{
    public class PagedRecords<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }

        public int RecordNumber { get; set; }

        public List<T> Records { get; set; }
    }
}
