using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Feature.Common;

namespace Core.Feature.Overtime.GetAllReportLeader
{
    public class QueryReportLeader
    {
        public string Solver { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.MinValue;
        public string Status { get; set; } = string.Empty;
    }
}