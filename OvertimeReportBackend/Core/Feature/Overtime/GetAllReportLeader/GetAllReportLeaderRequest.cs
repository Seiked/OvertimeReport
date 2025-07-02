using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.GetAllReportLeader
{
    public class GetAllReportLeaderRequest : IRequest<Response<List<GetAllReportLeaderDto>>>
    {
        public QueryReportLeader Query { get; set; }

        public GetAllReportLeaderRequest(QueryReportLeader query)
        {
            Query = query;
        }
    }
}