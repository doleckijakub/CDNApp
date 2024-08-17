using Microsoft.AspNetCore.StaticFiles;

namespace CDNApp.Helpers
{
    public static class PathHelper
    {
        public static string GetSavePath(string uuid)
        {
            return Path.GetFullPath(Path.Combine(Config.UploadsDirectory, uuid));
        }

        public static string GetUploadedFilePath(string uuid, string filename)
        {
            return Path.Combine(GetSavePath(uuid), filename);
        }

        public static string GetHttpContentType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType)) return "application/octet-stream";
            return contentType;
        }
    }
}
