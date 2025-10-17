using Microsoft.EntityFrameworkCore;
using GUI_Programmering_WebApi.Models;
using GUI_Programmering_WebApi;

MapsterConfiguration.ConfigureMappings();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder => builder
            .SetIsOriginAllowed((host) => true) // Tillader enhver host
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // Nødvendigt for SignalR
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(
 o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
