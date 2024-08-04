using Microsoft.AspNetCore.Mvc;
using NLog;
using Presentation;
using Presentation.ActionFilters;
using Services.Contracts;
using WebAPI.Extensions;
using WebAPI.Utilities.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LogManager.Setup().LoadConfigurationFromFile(String.Concat(Directory.GetCurrentDirectory(),"/nlog.config"));

builder.Services.AddControllers(
            config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            })
    .AddCustomCsvOutputFormatter()
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(AssemblyReference).Assembly)
    .AddNewtonsoftJson();

//ModelState invalid olduğunda 400 donmesin ancak bu durumda ModelState'in valid lup olmadigini check etmelisin
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureIBookRepository();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureBookService();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureActionFilters(); // IoC'ye action filteri verdik
builder.Services.ConfigureCors();
    
var app = builder.Build();

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILoggerService>());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();