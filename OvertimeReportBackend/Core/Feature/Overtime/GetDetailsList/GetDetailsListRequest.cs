using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Common;
using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.GetDetailsList
{
    public class GetDetailsListRequest : IRequest<Response<List<ListCommonStructure>>>
    {
        public GetDetailsListRequest()
        {
        }
    }
}