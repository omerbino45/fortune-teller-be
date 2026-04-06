using FortuneTeller.API.Middleware;
using FortuneTeller.Application.Interfaces;
using FortuneTeller.Application.Mappings;
using FortuneTeller.Application.Services;
using FortuneTeller.Domain.Interfaces;
using FortuneTeller.Infrastructure.Persistence;
using FortuneTeller.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("FortuneTeller.Infrastructure")));

// Application services
builder.Services.AddScoped<IWorryRepository, WorryRepository>();
builder.Services.AddScoped<IWorryService, WorryService>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<WorryProfile>());

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Fortune Teller API", Version = "v1" });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
