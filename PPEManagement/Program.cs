using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PPEManagement.Common;
using PPEManagement.Context;
using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Infrastructure;
using PPEManagement.Repositories;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Services;
using PPEManagement.Services.AutoMapper;
using PPEManagement.Services.Contracts;
using PPEManagement.Services.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Настраиваем сериализатор использовать camelCase (маленькие буквы), 
        // чтобы он идеально подходил под ваш текущий JS-код в представлениях
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddHttpContextAccessor();

// PostgreSQL
builder.Services.AddDbContext<PPEManagementContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(PPEManagementContext).Assembly.FullName);
        }));

builder.Services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<PPEManagementContext>());
builder.Services.AddScoped<IReader>(x => x.GetRequiredService<PPEManagementContext>());
builder.Services.AddScoped<IWriter>(x => x.GetRequiredService<PPEManagementContext>());
builder.Services.AddScoped<IDbWriterContext>(x => x.GetRequiredService<PPEManagementContext>());

// Провайдеры для аудит-полей (контекст получает их в конструктор)
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IIdentityProvider, HttpIdentityProvider>();

// Репозитории
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPPECardRepository, PPECardRepository>();
builder.Services.AddScoped<IPPEStatementRepository, PPEStatementRepository>();

// AutoMapper (это и была ваша ошибка)
builder.Services.AddAutoMapper(cfg => { }, typeof(ServiceProfile));

// Валидаторы: регистрирует все AbstractValidator<> из сборки Services
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeCreateModelValidator>();

// Сервисы
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPPECardService, PPECardService>();
builder.Services.AddScoped<IPPEStatementService, PPEStatementService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PpeStatement}/{action=Index}/{id?}");   // было PpeStatementController

app.Run();