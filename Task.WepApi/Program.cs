using Microsoft.EntityFrameworkCore;
using Task.Business.IService;
using Task.Business.Logger;
using Task.Business.Service;
using Task.DAL.Context;
using Task.DAL.IRepositories;
using Task.DAL.Repositories;
using Task.WepApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Add Db Context
builder.Services.AddDbContext<TaskDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionString:TaskConnection"], b => b.MigrationsAssembly("Task.DAL"));
});
#endregion
builder.Services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync>();
builder.Services.AddScoped<ICandidateContactRepository, CandidateContactRepository>();
builder.Services.AddScoped<ICandidateContactService, CandidateContactService>();
builder.Services.AddAutoMapper(typeof(Task.Business.Profile).Assembly);
builder.Services.AddSingleton<ILoggerService, LoggerService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) => {
    context.Request.EnableBuffering();

    await next();
});
app.ConfigureCustomRequestMiddleware();
app.ConfigureCustomExceptionMiddleware();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
