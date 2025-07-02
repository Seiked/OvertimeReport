using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Core.Contracts.Overtime;
using Core.Feature.Common;
using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.GetAllReportLeader
{
    public class GetAllReportLeaderRequestHandler : IRequestHandler<GetAllReportLeaderRequest, Response<List<GetAllReportLeaderDto>>>
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public GetAllReportLeaderRequestHandler(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<GetAllReportLeaderDto>>> Handle(GetAllReportLeaderRequest request, CancellationToken cancellationToken)
        {
            var queryReport = _mapper.Map<QueryReport>(request.Query);
            var reportLeaderList = await _reportRepository.GetAll(queryReport);
            var response = new Response<List<GetAllReportLeaderDto>>()
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = _mapper.Map<List<GetAllReportLeaderDto>>(reportLeaderList)
            };
            return response;
        }
    }
}