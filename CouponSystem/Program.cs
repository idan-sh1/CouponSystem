
using CouponSystem.Data;
using CouponSystem.Models;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
