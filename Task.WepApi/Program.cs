using Microsoft.EntityFrameworkCore;
using Task.Business.IService;
using Task.Business.Service;
using Task.DAL.Context;
using Task.DAL.IRepositories;
using Task.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Add Db Context
builder.Services.AddDbContext<TaskDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionString:TaskConnection"], b => b.MigrationsAssembly("Task.DAL"));
});
#endregion
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync>();
builder.Services.AddScoped<ICandidateContactRepository, CandidateContactRepository>();
builder.Services.AddScoped<ICandidateContactService, CandidateContactService>();
builder.Services.AddAutoMapper(typeof(Task.Business.Profile).Assembly);



var app = builder.Build();

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
