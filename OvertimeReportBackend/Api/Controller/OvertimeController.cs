using Core.Feature.Overtime.CreateReport;
using Core.Feature.Overtime.GetAllReportLeader;
using Core.Feature.Overtime.GetAllReportUser;
using Core.Feature.Overtime.GetDetailsList;
using Core.Feature.Overtime.GetReportById;
using Core.Feature.Overtime.UpdateApproveReport;
using Core.Feature.Overtime.UpdateRejectReport;
using Core.Models.Common;
using Core.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller
{
    [Route("api/overtime")]
    public class OvertimeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OvertimeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "TeamMember")]
        [HttpGet("details")]
        [Produces<Response<List<ListCommonStructure>>>]
        public async Task<ActionResult<Response<List<ListCommonStructure>>>> GetDetails()
        {
            return await _mediator.Send(new GetDetailsListRequest());
        }
        [Authorize(Roles = "TeamMember")]
        [HttpPost("report")]
        [Produces<Response<Unit>>]
        public async Task<ActionResult<Response<Unit>>> Create([FromBody] CreateReportDto reportDto)
        {
            return await _mediator.Send(new CreateReportRequest(reportDto));
        }
        [Authorize(Roles = "TeamMember")]
        [HttpGet("all/user")]
        [Produces<Response<List<GetAllReportUserDto>>>]
        public async Task<ActionResult<Response<List<GetAllReportUserDto>>>> GetAllByUser([FromQuery] QueryReportUser query)
        {
            return await _mediator.Send(new GetAllReportUserRequest(query));
        }
        [Authorize(Roles = "Leader")]
        [HttpGet("all")]
        [Produces<Response<List<GetAllReportLeaderDto>>>]
        public async Task<ActionResult<Response<List<GetAllReportLeaderDto>>>> GetAll([FromQuery] QueryReportLeader query)
        {
            return await _mediator.Send(new GetAllReportLeaderRequest(query));
        }

        [Authorize(Roles = "Leader,TeamMember")]
        [HttpGet("{id}")]
        [Produces<Response<GetReportByIdDto>>]
        public async Task<ActionResult<Response<GetReportByIdDto>>> GetById([FromRoute] int id)
        {
            return await _mediator.Send(new GetReportByIdRequest(id));
        }
        [Authorize(Roles = "Leader")]
        [HttpPut("approve/{id}")]
        [Produces<Response<Unit>>]
        public async Task<ActionResult<Response<Unit>>> UpdateApprovalReport([FromRoute] int id, [FromBody] UpdateApprovalReportDto data)
        {
            return await _mediator.Send(new UpdateApprovalReportRequest(id, data));
        }
        [Authorize(Roles = "Leader")]
        [HttpPut("reject/{id}")]
        [Produces<Response<Unit>>]
        public async Task<ActionResult<Response<Unit>>> UpdateRejectReport([FromRoute] int id, [FromBody] UpdateRejectReportDto data)
        {
            return await _mediator.Send(new UpdateRejectReportRequest(id, data));
        }
    }
}