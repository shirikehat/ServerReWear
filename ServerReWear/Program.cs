
using Microsoft.EntityFrameworkCore;
using ServerReWear.Models;

namespace ServerReWear
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            

            //Add Database to dependency injection
            builder.Services.AddDbContext<ShiriDBContext>(options => 
            options.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=ReWear_DB;User ID=AdminUser;Password=admin123; Trusted_Connection=True; MultipleActiveResultSets=true;"));

            

            #region Add Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Add Session
            app.UseSession(); //In order to enable session management
            #endregion 

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
