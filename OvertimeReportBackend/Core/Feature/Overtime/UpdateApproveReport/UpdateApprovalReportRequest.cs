using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.UpdateApproveReport
{
    public class UpdateApprovalReportRequest : IRequest<Response<Unit>>
    {
        public int Id { get; set; }
        public UpdateApprovalReportDto UpdateApprovalReportDto { get; set; }
        public UpdateApprovalReportRequest(int id, UpdateApprovalReportDto updateApprovalReportDto)
        {
            Id = id;
            UpdateApprovalReportDto = updateApprovalReportDto;
        }
    }
}