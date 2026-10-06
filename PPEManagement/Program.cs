using Microsoft.EntityFrameworkCore;
using PPEManagement.BLL.Services;
using PPEManagement.Context;
using PPEManagement.Dal.Contracts.Repositories; // Подключаем пространство имен для IDbWriterContext
using PPEManagement.Repositories;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Services;
using PPEManagement.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры
builder.Services.AddControllersWithViews();

// Настраиваем PostgreSQL
builder.Services.AddDbContext<PPEManagementContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(PPEManagementContext).Assembly.FullName);
        }));

// РЕШЕНИЕ: Регистрируем PPEManagementContext как реализацию интерфейса IDbWriterContext
builder.Services.AddScoped<IDbWriterContext>(provider => 
    provider.GetRequiredService<PPEManagementContext>());

// Регистрируем репозитории
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPPECardRepository, PPECardRepository>();
builder.Services.AddScoped<IPPEStatementRepository, PPEStatementRepository>();

// Регистрируем сервисы
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPPECardService, PPECardService>();
builder.Services.AddScoped<IPPEStatementService, PPEStatementService>();
// 1. Настраиваем PostgreSQL
builder.Services.AddDbContext<PPEManagementContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(PPEManagementContext).Assembly.FullName);
        }));

// 2. Регистрируем PPEManagementContext как реализацию интерфейса IDbWriterContext
builder.Services.AddScoped<IDbWriterContext>(provider => 
    provider.GetRequiredService<PPEManagementContext>());

// РЕШЕНИЕ: Регистрируем PPEManagementContext как реализацию интерфейса IReader
builder.Services.AddScoped<PPEManagement.Dal.Contracts.Repositories.IReader>(provider => 
    provider.GetRequiredService<PPEManagementContext>());

// 3. Регистрируем репозитории
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPPECardRepository, PPECardRepository>();
builder.Services.AddScoped<IPPEStatementRepository, PPEStatementRepository>();

var app = builder.Build();

// Настройка middleware
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();