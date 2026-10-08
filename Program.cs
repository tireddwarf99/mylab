using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Data;

var builder = WebApplication.CreateBuilder(args);
var localDatabase = builder.Environment.IsDevelopment();
if (localDatabase)
{
    builder.Services.AddDbContext<MyDatabaseContext>(options =>
        options.UseSqlite(builder.Configuration["LOCAL_DB_CONNECTION"] ?? "Data Source=lab2-local.db"));
}
else
{
    var sqlConnection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")
        ?? builder.Configuration["AZURE_SQL_CONNECTIONSTRING"]
        ?? throw new InvalidOperationException("Azure SQL connection string is required.");
    builder.Services.AddDbContext<MyDatabaseContext>(options => options.UseSqlServer(sqlConnection));
}
var redisConnection = builder.Configuration["AZURE_REDIS_CONNECTIONSTRING"];
if (!string.IsNullOrWhiteSpace(redisConnection))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConnection;
        options.InstanceName = "Lab2:";
    });
}
else if (localDatabase)
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    throw new InvalidOperationException("Azure Redis connection string is required.");
}
builder.Services.AddControllersWithViews();
builder.Logging.AddAzureWebAppDiagnostics();
var app = builder.Build();
if (localDatabase)
{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<MyDatabaseContext>().Database.EnsureCreated();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapGet("/health", () => Results.Ok(new { status = "ok", laboratory = 2 }));
app.MapControllerRoute(name: "default", pattern: "{controller=Todos}/{action=Index}/{id?}");
app.Run();
