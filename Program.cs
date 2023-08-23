using HotelListing.API.Configurations.AutoMapperConfig;
using HotelListing.API.Data;
using hotelListingAPI.Contracts;
using hotelListingAPI.data.Entities;
using hotelListingAPI.Middleware;
using hotelListingAPI.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

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


//we are adding the User model. 
//we are using the build in identity core package
//you can also add the context of the DB where you will have your store
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    //add the token provider to be able to generate and do stuff with refresh token provider
    .AddTokenProvider<DataProtectorTokenProvider<User>>("HotelListingApi")
    .AddEntityFrameworkStores<HotelListingDbContext>()
    .AddDefaultTokenProviders();

//you are registering these in program.cs to be able to inject it anywhere in your application.
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
//There are three types: AddScoped, AddTransient and AddSingleton
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();


builder.Services.AddAuthentication(options =>
{
    //config
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //Bearer
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //Bearer 
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //parameters to impose security
        //encode the secret key and check against that key
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        //no offset on the time
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        //secret key
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});
//register AutoMapper as a service for it to be injectible
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024;
    options.UseCaseSensitivePaths = true;
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

builder.Services.AddApiVersioning(options =>
{
    //default api version if not specified is defined as 1.0
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;

    //define the different places you can put the api version in the request
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));


});

//this is the format in which you will be adding your versions in
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
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

//add the custom middleware exception
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<CachingMiddleware>();
//use this middleware to log requests that are getting hit on the server
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseResponseCaching();


// use the policy we defined
app.UseCors("AllowAll");

//add this middleware to actually validate the incoming tokens to protect endpoints
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
