
using MongoDb_CRUD.Models;
using MongoDb_CRUD.Services.Interfaces;
using MongoDb_CRUD.Services.Respositories;

namespace MongoDb_CRUD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // AiTiman Database.
            builder.Services.Configure<AiTimanDatabaseSettings>(
                builder.Configuration.GetSection("AiTimanDatabase"));

            // Add Scope of Interface and Repository
            builder.Services.AddScoped<IAppointment, AppointmentRepository>();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
