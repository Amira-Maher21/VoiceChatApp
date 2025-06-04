
using Microsoft.EntityFrameworkCore;
using VoiceChatApp.Data;
using VoiceChatApp.Hubs;
using VoiceChatApp.Service;

namespace VoiceChatApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // SignalR
            builder.Services.AddSignalR();





            builder.Services.AddScoped<CallLogService>();
 



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Serve wwwroot files
            app.UseDefaultFiles();  // مهم عشان يقرأ index.html
            app.UseStaticFiles();

            // SignalR endpoint
            app.MapHub<ChatHub>("/chatHub");
 

            // fallback to index.html
            app.MapFallbackToFile("index.html");


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
