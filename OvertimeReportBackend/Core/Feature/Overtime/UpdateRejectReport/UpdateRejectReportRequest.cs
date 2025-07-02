using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.UpdateRejectReport
{
    public class UpdateRejectReportRequest : IRequest<Response<Unit>>
    {
        public int Id { get; set; }
        public UpdateRejectReportDto UpdateRejectReportDto { get; set; }
        public UpdateRejectReportRequest(int id, UpdateRejectReportDto updateRejectReportDto)
        {
            Id = id;
            UpdateRejectReportDto = updateRejectReportDto;
        }
    }
}