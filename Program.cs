using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CDNApp
{
    public class Program
    {
        private static readonly long MaxFileSize = 1L * 1024 * 1024 * 1024;
        private static readonly string Domain = "http://localhost:8080";
        private static readonly string UploadsDirectory = "uploads";

        private static readonly string UrlUploadPut = "/{filename}";
        // private static readonly string UrlUploadPutPrivate = "/private/{filename}"; // TODO
        private static readonly string UrlUploadGet = "/{uuid}/{filename}";
        private static readonly string UrlUploadList = "/upload/{uuid}";

        private static readonly IList<string> ReactEndpoints = new ReadOnlyCollection<string>
        (new[]
            {
                "/",
                UrlUploadList
            }
        );

        private static readonly string UrlApiV1AllUploads = "/api/v1/all";
        private static readonly string UrlApiV1FilesOf    = "/api/v1/filesof/{uuid}";

        public record UploadPutResult(string uuid, string filename)
        {
            public record Description(string directUrl, string websiteUrl);

            public Description GetDescription() {
                return new Description(
                    Domain + UrlUploadGet
                        .Replace("{uuid}", uuid)
                        .Replace("{filename}", filename),
                    Domain + UrlUploadList
                        .Replace("{uuid}", uuid)
                );
            }
        }

        public record Upload(string uuid, long fileSize, string lastModified);

        public static void Main(string[] args)
        {
            Directory.CreateDirectory(UploadsDirectory);

            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "frontend", "build")),
                RequestPath = ""
            });

            app.MapPut(UrlUploadPut, HandleFileUpload)
                .WithName("UploadPut")
                .WithOpenApi();

            app.MapGet(UrlUploadGet, HandleFileDownload)
                .WithName("UploadGet")
                .WithOpenApi();

            app.MapGet(UrlApiV1AllUploads, HandleApiAll)
                .WithName("ApiV1AllUploads")
                .WithOpenApi();

            app.MapGet(UrlApiV1FilesOf, HandleApiFilesOf)
                .WithName("ApiV1FilesOf")
                .WithOpenApi();

            foreach (string url in ReactEndpoints)
            {
                app.MapGet(url, async context =>
                {
                    await context.Response.SendFileAsync(Path.Combine(Directory.GetCurrentDirectory(), "frontend", "build", "index.html"));
                });
            }

            app.Run();

        }

        private static string GetSavePath(string uuid)
        {
            return Path.GetFullPath(Path.Combine(UploadsDirectory, uuid));
        }

        private static string GetUploadedFilePath(string uuid, string filename) 
        {
            return Path.Combine(GetSavePath(uuid), filename);
        }

        private static string GetHttpContentType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType)) return "application/octet-stream";
            return contentType;
        }

        private static async Task<IResult> HandleFileUpload(HttpContext context, string filename)
        {
            if (context.Request.ContentLength > MaxFileSize)
            {
                return Results.BadRequest("File size exceeds the limit.");
            }

            var uuid = Guid.NewGuid().ToString();
            var savePath = GetSavePath(uuid);

            Directory.CreateDirectory(savePath);

            var filePath = GetUploadedFilePath(uuid, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await context.Request.Body.CopyToAsync(stream);
            }

            return Results.Ok(new UploadPutResult(uuid, filename));
        }

        private static IResult HandleFileDownload(HttpContext context, string uuid, string filename)
        {
            var filePath = GetUploadedFilePath(uuid, filename);

            if (!System.IO.File.Exists(filePath))
            {
                return Results.NotFound("File not found.");
            }

            var contentType = GetHttpContentType(filePath);

            return Results.File(filePath, contentType, filename);
        }

        private static IResult HandleApiAll(HttpContext context)
        {
            var uuids = Directory
                .GetDirectories(UploadsDirectory)
                .Select(dir => 
                {
                    var dirInfo = new DirectoryInfo(dir);
                    var uuid = dirInfo.Name;
                    var fileSize = dirInfo.EnumerateFiles("*", SearchOption.AllDirectories).Sum(f => f.Length);
                    var lastModified = dirInfo.LastWriteTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                    return new Upload(uuid, fileSize, lastModified);
                })
                .ToArray();

            return Results.Ok(uuids);
        }

        private static IResult HandleApiFilesOf(HttpContext context, string uuid)
        {
            var uploadPath = GetSavePath(uuid);

            if (!Directory.Exists(uploadPath))
            {
                return Results.NotFound("Upload not found.");
            }

            var files = Directory
                .GetFiles(uploadPath)
                .Select(Path.GetFileName)
                .ToArray();

            return Results.Ok(files);
        }
    }
}
