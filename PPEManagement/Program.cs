using Microsoft.EntityFrameworkCore;
using PPEManagement.Context;
using PPEManagement.Dal.Contracts.Repositories; 
using PPEManagement.Repositories;
using PPEManagement.Repositories.Contracts;
using PPEManagement.Services;
using PPEManagement.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// 1. Добавляем контроллеры и представления (MVC)
builder.Services.AddControllersWithViews();

// 2. Настраиваем PostgreSQL
builder.Services.AddDbContext<PPEManagementContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(PPEManagementContext).Assembly.FullName);
        }));

builder.Services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<PPEManagementContext>());
builder.Services.AddScoped<IReader>(x => x.GetRequiredService<PPEManagementContext>());
builder.Services.AddScoped<IDbWriterContext>(x => x.GetRequiredService<PPEManagementContext>());

// 5. Регистрируем репозитории
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPPECardRepository, PPECardRepository>();
builder.Services.AddScoped<IPPEStatementRepository, PPEStatementRepository>();

// 6. Регистрируем бизнес-сервисы
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPPECardService, PPECardService>();
builder.Services.AddScoped<IPPEStatementService, PPEStatementService>();

var app = builder.Build();

// 7. Настройка Middleware
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
    pattern: "{controller=PpeStatementController}/{action=Index}/{id?}");

app.Run();