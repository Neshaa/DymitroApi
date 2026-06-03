
using Intelisale.DymitroApi.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthorizedControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorizedSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureMapster();
builder.Services.AddServices();
builder.Services.AddMetrics();
//builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.ConfigureDapperDbContext(builder.Configuration);
builder.Services.AddOptions(builder.Configuration);
//builder.Services.AddCustomLogger(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddCors(options => options.AddPolicy("Default", p => p.AllowAnyOrigin()
                                                                           .AllowAnyHeader()
                                                                           .AllowAnyMethod()));

var app = builder.Build();

app.UseCors("Default");
app.ConfigureSwaggerUse();


//app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapControllers();

app.Run();
