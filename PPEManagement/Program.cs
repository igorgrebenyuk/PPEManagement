using Microsoft.EntityFrameworkCore;
using PPEManagement.BLL.Services;
using PPEManagement.Context;
using PPEManagement.Dal.Contracts.Repositories;
using PPEManagement.Repositories;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Services;
using PPEManagement.Services.Contracts;
using PPEManagement.Common; // Пространство имен для провайдеров времени и идентификации

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры с представлениями
builder.Services.AddControllersWithViews();

// 1. Настраиваем PostgreSQL
builder.Services.AddDbContext<PPEManagementContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(PPEManagementContext).Assembly.FullName);
        }));

// 2. Регистрируем базовые инфраструктурные провайдеры для контекста
// (Если у вас есть их реальные классы реализации, замените эти заглушки на них, например: .AddScoped<IDateTimeProvider, DateTimeProvider>())
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped();

// 3. Регистрируем PPEManagementContext как реализацию интерфейсов доступа к данным
builder.Services.AddScoped<IDbWriterContext>(provider => 
    provider.GetRequiredService<PPEManagementContext>());

builder.Services.AddScoped<IReader>(provider => 
    provider.GetRequiredService<PPEManagementContext>());

// 4. Регистрируем репозитории
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPPECardRepository, PPECardRepository>();
builder.Services.AddScoped<IPPEStatementRepository, PPEStatementRepository>();

// 5. Регистрируем бизнес-сервисы
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPPECardService, PPECardService>();
builder.Services.AddScoped<IPPEStatementService, PPEStatementService>();

var app = builder.Build();

// Настройка middleware среды выполнения
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
