using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BackPanel.Domain.Enums;

namespace BackPanel.Application.DTOsRequests;
public class SearchExpressionDtoRequest
{
    [Required]
    public string? PropName { get; set; }
    [Required]
    public ComparisonOperator Operator { get; set; }
    [Required]
    public string? PropValue { get; set; }
}
