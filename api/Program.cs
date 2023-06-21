using api.Helpers;
using api.Models;
using api.Repo;
using api.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System.Net.Mime;

/* TODO:
 * Sutvarkyti repository
 * Sutvarkyti servisus
 * Sutvarkyti DTOs
 * Prideti lokalizacija
 * Reikia atskiro objekto produktu kainoms
 */


var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddAutoMapper(typeof(Program));

services.AddMvc(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("Local"),
        name: "sqlserver",
        tags: new string[] { "ready" },
        timeout: TimeSpan.FromSeconds(3));

services.AddCors();

services.AddControllers()
        .AddNewtonsoftJson((options) =>
            options.SerializerSettings
            .ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

services.AddDbContext<ApplicationDbContext>(options => options
    //.UseLazyLoadingProxies()
    .UseSqlServer(builder.Configuration.GetConnectionString("Local")));

services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 0;
}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
});

services.AddAuthorization();

//services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtConfiguration"));
services.AddAuthentication();/* (options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", jwtBearerOptions =>
{
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtConfiguration:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2),
    };
});*/

services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//services.AddScoped<IJwtService, JwtService>();
services.AddScoped<IImageService, ImageService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IItemService, ItemService>();
services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<ICartService, CartService>();
services.AddScoped<IWishlistItemService, WishlistItemService>();
services.AddScoped<IReviewService, ReviewService>();
services.AddScoped<IUnitOfWork, UnitOfWork>();

//services.AddScoped<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options
    .WithOrigins(new[] { "http://localhost:3000", "https://localhost:8000", "http://localhost:4200", })
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

// add roles and power user
SeedData.InitializeRolesAndAdmin(app.Services, app.Configuration).Wait();

var env = builder.Environment;
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

// health checks
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        var result = System.Text.Json.JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString(),
                })
            });

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = (_) => false,
});

app.Run();