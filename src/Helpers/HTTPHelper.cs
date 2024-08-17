using Microsoft.AspNetCore.StaticFiles;

namespace CDNApp.Helpers
{
    public static class HTTPHelper
    {
        public static string GetHttpContentType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType)) return "application/octet-stream";
            return contentType;
        }
    }
}
