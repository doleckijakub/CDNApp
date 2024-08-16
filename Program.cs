using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CDNApp
{
    public class Program
    {
        private const long MaxFileSize = 1L * 1024 * 1024 * 1024;
        private const string Domain = "http://localhost:8080";
        private const string UploadsDirectory = "uploads";

        private const string UrlUploadPut = "/{filename}";
        // private const string UrlUploadPutPrivate = "/private/{filename}"; // TODO
        private const string UrlUploadGet = "/{uuid}/{filename}";
        private const string UrlUploadList = "/upload/{uuid}";

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

        public static void Main(string[] args)
        {
            Directory.CreateDirectory(UploadsDirectory);

            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapPut(UrlUploadPut, HandleFileUpload)
               .WithName("UploadPut")
               .WithOpenApi();

            app.MapGet(UrlUploadList, HandleFileList)
               .WithName("UploadList")
               .WithOpenApi();

            app.MapGet(UrlUploadGet, HandleFileDownload)
               .WithName("UploadGet")
               .WithOpenApi();

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

        private static IResult HandleFileList(HttpContext context, string uuid)
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
    }
}
