using AutoMapper;
using Core.Feature.Common;
using Core.Feature.Overtime.CreateReport;
using Core.Feature.Overtime.GetAllReportLeader;
using Core.Feature.Overtime.GetAllReportUser;
using Core.Feature.Overtime.GetReportById;
using Core.Feature.Overtime.UpdateApproveReport;
using Core.Feature.Overtime.UpdateRejectReport;
using Core.Models.Common;
using Core.Models.Overtime;

namespace Core.MappingProfile
{
    public class OvertimeProfile : Profile
    {
        public OvertimeProfile()
        {
            CreateMap<Detail, ListCommonStructure>()
            .ForMember(des => des.Value, opt => opt.MapFrom(src => src.Id))
            .ForMember(des => des.Text, opt => opt.MapFrom(src => src.Name));
            CreateMap<CreateReportDto, Report>();
            CreateMap<QueryReportLeader, QueryReport>();
            CreateMap<QueryReportUser, QueryReport>();
            CreateMap<Report, GetAllReportLeaderDto>()
            .ForMember(des => des.ReportDate, opt => opt.MapFrom(src => src.NoveltyDate.ToShortDateString()))
            .ForMember(des => des.NotificationDate, opt => opt.MapFrom(src => src.CreationDate.ToShortDateString()))
            .ForMember(des => des.SolId, opt => opt.MapFrom(src => src.ApplicationUser.SolId))
            .ForMember(des => des.SolName, opt => opt.MapFrom(src => src.ApplicationUser.Name))
            .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(des => des.Detail, opt => opt.MapFrom(src => src.Detail.Name));
            CreateMap<Report, GetReportByIdDto>()
            .ForMember(des => des.SolHq, opt => opt.MapFrom(src => src.ApplicationUser.Office))
            .ForMember(des => des.SolId, opt => opt.MapFrom(src => src.ApplicationUser.SolId))
            .ForMember(des => des.SolName, opt => opt.MapFrom(src => src.ApplicationUser.Name))
            .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(des => des.ReportHq, opt => opt.MapFrom(src => src.Headquarter))
            .ForMember(des => des.Detail, opt => opt.MapFrom(src => src.Detail.Name));
            CreateMap<UpdateApprovalReportDto, Report>()
            .ForMember(des => des.ApprovalDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(des => des.StatusId, opt => opt.MapFrom(src => 2));
            CreateMap<UpdateRejectReportDto, Report>()
            .ForMember(des => des.ApprovalDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(des => des.StatusId, opt => opt.MapFrom(src => 3));
        }
    }
}