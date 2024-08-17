using Microsoft.AspNetCore.Mvc;
using CDNApp.Helpers;
using CDNApp.Models;

namespace CDNApp.Controllers
{
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPut(Config.UrlUploadPut)]
        public async Task<IActionResult> HandleFileUpload(string filename)
        {
            if (Request.ContentLength > Config.MaxFileSize)
            {
                return BadRequest("File size exceeds the limit.");
            }

            var uuid = Guid.NewGuid().ToString();
            var savePath = Config.GetSavePath(uuid);

            Directory.CreateDirectory(savePath);

            var filePath = Config.GetUploadedFilePath(uuid, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Request.Body.CopyToAsync(stream);
            }

            return Ok(new UploadPutResult(uuid, filename));
        }

        [HttpGet(Config.UrlUploadGet)]
        public IActionResult HandleFileDownload(string uuid, string filename)
        {
            var filePath = Config.GetUploadedFilePath(uuid, filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var contentType = HTTPHelper.GetHttpContentType(filePath);

            return File(System.IO.File.OpenRead(filePath), contentType, filename);
        }
    }
}
