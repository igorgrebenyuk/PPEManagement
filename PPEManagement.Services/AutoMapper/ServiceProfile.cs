using AutoMapper;
using PPEManagement.Entities;
using PPEManagement.Services.Contracts.Models.Employee;
using PPEManagement.Services.Contracts.Models.PPECard;
using PPEManagement.Services.Contracts.Models.PPEStatement;

namespace PPEManagement.Services.AutoMapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMapForEmployee();
        CreateMapForPPECard();
        CreateMapForPPEStatement();
    }

    private void CreateMapForEmployee()
    {
        CreateMap<Employee, EmployeeModel>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName));

        CreateMap<EmployeeCreateModel, Employee>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName} {src.MiddleName}".Trim()));

        CreateMap<EmployeeUpdateModel, Employee>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName} {src.MiddleName}".Trim()));
    }

    private void CreateMapForPPECard()
    {
        CreateMap<PPECard, PPECardModel>();
        CreateMap<PPECardCreateModel, PPECard>();
        CreateMap<PPECardUpdateModel, PPECard>();
    }

    private void CreateMapForPPEStatement()
    {
        // Маппинг списочной модели
        CreateMap<PPEStatement, PPEStatementModel>()
            .ForMember(dest => dest.StatementNumber, opt => opt.MapFrom(src => src.StatementNumber))
            .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
            .ForMember(dest => dest.EmployeeFullName, opt => opt.Ignore());

        // Маппинг детальной модели
        CreateMap<PPEStatement, PPEStatementDetailModel>()
            .ForMember(dest => dest.StatementNumber, opt => opt.MapFrom(src => src.StatementNumber))
            .ForMember(dest => dest.Employee, opt => opt.Ignore()) // Игнорируем Employee в шапке
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        // Маппинг строки табличной части
        CreateMap<PPEStatementItem, PPEStatementItemModel>()
            .ForMember(dest => dest.PPECardId, opt => opt.Ignore())
            .ForMember(dest => dest.MeasureUnit, opt => opt.MapFrom(_ => "шт."));

        // Маппинги для создания
        CreateMap<PPEStatementCreateModel, PPEStatement>()
            .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.StatementDate));

        CreateMap<PPEStatementItemCreateModel, PPEStatementItem>();
    }
}