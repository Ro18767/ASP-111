using ASP_111.Data;
using ASP_111.Middleware;
using ASP_111.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IDateService, DateService>();

builder.Services.AddSingleton<ValidatorService>();

String? connectionString =
    builder.Configuration.GetConnectionString("PlanetScale");
MySqlConnection connection = new(connectionString);
builder.Services.AddDbContext<DataContext>(
    options =>
        options.UseMySql(
            connection,
            ServerVersion.AutoDetect(connection),
            serverOptions =>
                serverOptions
                    .MigrationsHistoryTable(
                        tableName: HistoryRepository.DefaultTableName,
                        schema: "asp111")
                    .SchemaBehavior(
                        MySqlSchemaBehavior.Translate,
                        (schema, table) => $"{schema}_{table}")
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// test
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMarker();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
