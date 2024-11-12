
using CouponSystem.Data;
using CouponSystem.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;

namespace CouponSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ----------------------------------------------------------------- //
                               // Add services to the container
            // ----------------------------------------------------------------- //

            // Add EF Core with SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity services
            builder.Services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false; // disable special chars password requirement
                opt.Password.RequireUppercase = false; // disable uppercase letter password requirement
            })
                .AddEntityFrameworkStores<AppDbContext>();

            // Add Enable CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            // Store the JWT Settings in variable
            var jwtSettings = builder.Configuration.GetSection("JWTSettings");

            // Add JWT Authentication
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["validIssuer"],
                    ValidAudience = jwtSettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(jwtSettings.GetSection("securityKey").Value!))
                };

            });

            // Add JwtHandler service
            builder.Services.AddSingleton<JwtHandler>();

            // Add controllers and endpoints API explorer
            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // ignore reference cycles to prevent self-referencing loops in objects with relationships.

            builder.Services.AddEndpointsApiExplorer();

            // Add authorization support to Swagger UI
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // ----------------------------------------------------------------- //
            // Build the application
            // ----------------------------------------------------------------- //

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use "EnableCORS" policy
            app.UseCors("EnableCORS");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
