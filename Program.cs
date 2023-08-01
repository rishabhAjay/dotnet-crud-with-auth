using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add the connection string to the db via the appsettings file
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
builder.Services.AddDbContext<HotelListing.API.Data.HotelListingDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

//create a policy based on the requirements for your cors setup
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        b =>
        {
            b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

//configure serilog to write to console and read config from the context of configuration file(appsettings.json)
builder.Host.UseSerilog(
    (ctx, config) => config.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration)
);

//we want the builder to build the backend server before going ahead with defining middlewares
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//use this middleware to log requests that are getting hit on the server
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

// use the policy we defined
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
