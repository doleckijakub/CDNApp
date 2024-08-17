using Microsoft.AspNetCore.Mvc;
using CDNApp.Helpers;
using CDNApp.Models;
using System.Globalization;

namespace CDNApp.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet(Config.UrlApiV1AllUploads)]
        public IActionResult HandleApiAll()
        {
            var uuids = Directory
                .GetDirectories(Config.UploadsDirectory)
                .Select(dir =>
                {
                    var dirInfo = new DirectoryInfo(dir);

                    var uuid = dirInfo.Name;
                    var fileSize = dirInfo.EnumerateFiles("*", SearchOption.AllDirectories).Sum(f => f.Length);
                    var lastModified = dirInfo.LastWriteTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                    return new Upload(uuid, fileSize, lastModified);
                })
                .ToArray();

            return Ok(uuids);
        }

        [HttpGet(Config.UrlApiV1FilesOf)]
        public IActionResult HandleApiFilesOf(string uuid)
        {
            var uploadPath = PathHelper.GetSavePath(uuid);

            if (!Directory.Exists(uploadPath))
            {
                return NotFound("Upload not found.");
            }

            var files = Directory
                .GetFiles(uploadPath)
                .Select(file =>
                {
                    var fileInfo = new FileInfo(file);

                    var filename = fileInfo.Name;
                    var fileSize = fileInfo.Length;

                    return new Models.File(filename, fileSize);
                })
                .ToArray();

            return Ok(files);
        }
    }
}
