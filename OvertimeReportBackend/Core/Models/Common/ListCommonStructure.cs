using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models.Common
{
    public class ListCommonStructure
    {
        public long Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}