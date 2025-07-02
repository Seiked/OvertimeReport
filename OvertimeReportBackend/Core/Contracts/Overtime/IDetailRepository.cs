using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Common;
using Core.Models.Overtime;

namespace Core.Contracts.Overtime
{
    public interface IDetailRepository
    {
        public Task<List<Detail>> GetDetails();
    }
}