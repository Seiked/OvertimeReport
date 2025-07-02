using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.GetReportById
{
    public class GetReportByIdRequest : IRequest<Response<GetReportByIdDto>>
    {
        public int Id { get; set; }

        public GetReportByIdRequest(int id)
        {
            Id = id;
        }
    }
}