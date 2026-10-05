using AutoMapper;
using PPEManagement.Services.Contracts.Models.Employee;
using PPEManagement.Services.Contracts.Models.PPECard;
using PPEManagement.Services.Contracts.Models.PPEStatement;
using PPEManagement.Entities;

namespace PPEManagement.Services.AutoMapper;

/// <summary>
    /// Профиль маппера сервисного слоя
    /// </summary>
    public class ServiceProfile : Profile
    {
        /// <summary>
        /// ctor
        /// </summary>
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
            // Маппинг шапки ведомости для списков
            CreateMap<PPEStatement, PPEStatementModel>()
                .ForMember(
                    dest => dest.StatementNumber,
                    opt => opt.MapFrom(src => src.StatementNumber));

            // Детальный маппинг ведомости со строками
            CreateMap<PPEStatement, PPEStatementDetailModel>()
                .ForMember(
                    dest => dest.Items,
                    opt => opt.MapFrom(src => src.Items)); // Использует коллекцию Items из PPEStatement

            // Маппинг строки ведомости
            CreateMap<PPEStatementItem, PPEStatementItemModel>()
                .ForMember(
                    dest => dest.PPEName,
                    opt => opt.MapFrom(src => src.PPEName)) // Читает напрямую из свойства PPEName строки
                .ForMember(
                    dest => dest.MeasureUnit,
                    opt => opt.MapFrom(src => "шт.")); // В вашей сущности нет поля единицы измерения, передаем стандартное значение
        }
    }