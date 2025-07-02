using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Core.Contracts.Overtime;
using Core.Models.Common;
using Core.Models.Response;
using MediatR;

namespace Core.Feature.Overtime.GetDetailsList
{
    public class GetDetailsListRequestHandler : IRequestHandler<GetDetailsListRequest, Response<List<ListCommonStructure>>>
    {
        private readonly IMapper _mapper;
        private readonly IDetailRepository _detailRepository;
        public GetDetailsListRequestHandler(IMapper mapper, IDetailRepository detailRepository)
        {
            _mapper = mapper;
            _detailRepository = detailRepository;
        }

        public async Task<Response<List<ListCommonStructure>>> Handle(GetDetailsListRequest request, CancellationToken cancellationToken)
        {
            var detailList = await _detailRepository.GetDetails();
            var commonStructureList = _mapper.Map<List<ListCommonStructure>>(detailList);
            var response = new Response<List<ListCommonStructure>>()
            {
                Data = commonStructureList,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
            return response;

        }
    }
}