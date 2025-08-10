using BackPanel.Application.DTOsRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPanel.Application.DTOs.Filters
{
    public class ListFilter
    {
        public PaginationFilter paginationFilter { get; set; } = new PaginationFilter();
        public string OrderBy { get; set; } = "LastUpdate";
        public bool Descending { get; set; } = true;
        public string Search { get; set; } = string.Empty;
        public List<SearchExpressionDtoRequest> SearchExpressions { get; set; } = new();
    }
}
