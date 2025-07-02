using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Feature.Overtime.GetAllReportUser
{
    public class QueryReportUser
    {
        public DateTime CreationDate { get; set; } = DateTime.MinValue;
        public string Status { get; set; } = string.Empty;
    }
}