using Core.MapperProfiles;
using Core.Services;
using Core.Interfaces;
using Data.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Middlewares;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("HospitalDbContextConnection") ?? throw new InvalidOperationException("Connection string 'HospitalDbContextConnection' not found.");

builder.Services.AddDbContext<HospitalDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// fluent validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAutoMapper(typeof(AppProfile));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


// custom services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDoctorsService, DoctorsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
