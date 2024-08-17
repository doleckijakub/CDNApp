using Microsoft.Extensions.FileProviders;
using System.Collections.ObjectModel;

namespace CDNApp
{
    public class Config
    {
        public const long MaxFileSize = 1L * 1024 * 1024 * 1024;
        public const string Domain = "http://localhost:8080";
        public const string UploadsDirectory = "uploads";

        // Upload/Download
        public const string UrlUploadPut = "/{filename}";
        public const string UrlUploadGet = "/{uuid}/{filename}";

        // Api
        public const string UrlApiV1AllUploads = "/api/v1/all";
        public const string UrlApiV1FilesOf = "/api/v1/filesof/{uuid}";

        // Web
        public const string UrlUploadList = "/upload/{uuid}";

        public static readonly IList<string> ReactEndpoints = new ReadOnlyCollection<string>(new[]
            {
                "/",
                UrlUploadList
            }
        );

        
    }
}
