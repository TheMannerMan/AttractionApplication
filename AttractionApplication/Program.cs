using Models;
using Services;
using Configuration;
using System.Runtime.InteropServices;
using DbRepos;

var builder = WebApplication.CreateBuilder(args);

// global cors policy
builder.Services.AddCors();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Dependency Inject Custom logger
//builder.Services.AddSingleton<ILoggerProvider, csInMemoryLoggerProvider>();
#endregion

#region Dependency Inject
//builder.Services.AddSingleton<IAnimalsService,csAnimalsService2>();
builder.Services.AddScoped<IUserAttractionService,csUserAttractionService>();
builder.Services.AddScoped<IUserAttractionRepo, csUserAttractionRepo>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// global cors policy - the call to UseCors() must be done here
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials


app.MapControllers();


/*
var _service = new WeatherService();
app.MapGet("/WeeklyForecast", () =>_service.WeeklyForecast());
app.MapGet("/BiWeeklyForecast", () =>_service.BiWeeklyForecast());
*/

app.Run();
