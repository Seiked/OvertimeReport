using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.CreateReport
{
    public class CreateReportRequest : IRequest<Response<Unit>>
    {
        public CreateReportDto CreateReportDto { get; set; }

        public CreateReportRequest(CreateReportDto createReportDto)
        {
            CreateReportDto = createReportDto;
        }
    }
}