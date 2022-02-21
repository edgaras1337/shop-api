using api.Data;
using api.Helpers;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddAutoMapper(typeof(Program));

services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        name: "sqlserver",
        tags: new string[] { "ready" },
        timeout: TimeSpan.FromSeconds(3));

services.AddCors();

services.AddControllers()
        .AddNewtonsoftJson((options) =>
            options.SerializerSettings
            .ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));//, ServiceLifetime.Transient);

services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtConfiguration"));

services.AddAuthentication(options =>
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
});

services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUserRoleRepository, UserRoleRepository>();
services.AddScoped<IRoleRepository, RoleRepository>();
services.AddScoped<IJwtService, JwtService>();
services.AddScoped<IImageService, ImageService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IItemRepository, ItemRepository>();
services.AddScoped<IItemService, ItemService>();
services.AddScoped<IItemImageRepository, ItemImageRepository>();
services.AddScoped<ICategoryRepository, CategoryRepository>();
services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<ICartRepository, CartRepository>();
services.AddScoped<ICartItemRepository, CartItemRepository>();
services.AddScoped<ICartService, CartService>();
services.AddScoped<IWishlistItemRepository, WishlistItemRepository>();
services.AddScoped<IWishlistItemService, WishlistItemService>();
services.AddScoped<ICommentRepository, CommentRepository>();
services.AddScoped<ICommentService, CommentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options
    .WithOrigins(new[] { "http://localhost:3000", "http://localhost:8080", "http://localhost:4200" })
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseHttpsRedirection();

var env = builder.Environment;

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "Images")),
    RequestPath = "/Images"
});

app.UseAuthentication();
//app.UseRouting();
app.UseAuthorization();

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