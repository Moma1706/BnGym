using System;

namespace Application.Common.Models.BaseResult
{
    public class PageResult<T>
    {
        public int Count { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
        public int ActiveCount { get; set; } = 0;
        public int NumberOfDayliArrivalsCurrentMonth { get; set; } = 0;
        public int NumberOfDayliArrivalsLastMonth { get; set; } = 0;
    }
}