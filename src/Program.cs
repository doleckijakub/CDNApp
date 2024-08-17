using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;

namespace CDNApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.CreateDirectory(Config.UploadsDirectory);

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "frontend", "build")),
                RequestPath = ""
            });

            app.MapControllers();

            foreach (string url in Config.ReactEndpoints)
            {
                app.MapGet(url, async context =>
                {
                    await context.Response.SendFileAsync(Path.Combine(Directory.GetCurrentDirectory(), "frontend", "build", "index.html"));
                });
            }

            app.Run();
        }
    }
}
