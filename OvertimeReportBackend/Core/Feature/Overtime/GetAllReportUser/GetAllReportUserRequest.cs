using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.GetAllReportUser
{
    public class GetAllReportUserRequest : IRequest<Response<List<GetAllReportUserDto>>>
    {
        public QueryReportUser Query;

        public GetAllReportUserRequest(QueryReportUser query)
        {
            Query = query;
        }
    }
}