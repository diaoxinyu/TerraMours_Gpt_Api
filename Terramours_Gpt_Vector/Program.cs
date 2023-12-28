using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Terramours_Gpt_Vector.Commons;
using Terramours_Gpt_Vector.IService;
using Terramours_Gpt_Vector.Service;

var builder = WebApplication.CreateBuilder(args);



var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;
IConfiguration configuration = builder.Configuration;
var connectionString = configuration.GetValue<string>("DbConnectionString");
//注入服务
builder.Services.AddScoped<IVectorService,VectorService>();
//初始化
builder.Services.AddTransient<DbInitialiser>();
//数据库
builder.Services.AddDbContext<EFCoreDbContext>(opt => {
    //从配置文件中获取key,这种方法需要新增一个类与之对应

    //var connStr = $"Host=localhost;Database=TerraMours;Username=postgres;Password=root";
    var connStr = connectionString;
    opt.UseNpgsql(connStr, o => o.UseVector());
    //设置EF默认AsNoTracking,EF Core不 跟踪
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    if (isDev)
    {
        //启用此选项后，EF Core将在日志中包含敏感数据，例如实体的属性值。这对于调试和排查问题非常有用。
        opt.EnableSensitiveDataLogging();
    }

    opt.EnableDetailedErrors();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
//初始化数据库

var initialiser = services.GetRequiredService<DbInitialiser>();

initialiser.Run();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
