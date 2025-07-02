using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Feature.Common
{
    public class QueryReport
    {
        public string Solver { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.MinValue;
        public string Status { get; set; } = string.Empty;
    }
}