using Application.Commons.Models;

var builder = WebApplication.CreateBuilder(args);

// appsettings.json
var appsettings = builder.Configuration.Get<Appsettings>() ?? new Appsettings();
builder.Services.AddSingleton(appsettings);

// add services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(appsettings);
builder.Services.AddApiServices(appsettings);

var app = builder.Build();
// use services
app.UseApiServices();

app.Run();

// https://stackoverflow.com/questions/69991983/deps-file-missing-for-dotnet-6-integration-tests
public partial class Program { }
