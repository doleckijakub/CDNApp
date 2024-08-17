namespace CDNApp.Models
{
    public record UploadPutResult(string uuid, string filename)
    {
        public record Description(string directUrl, string websiteUrl);

        public Description GetDescription()
        {
            return new Description(
                Config.Domain + Config.UrlUploadGet
                    .Replace("{uuid}", uuid)
                    .Replace("{filename}", filename),
                Config.Domain + Config.UrlUploadList
                    .Replace("{uuid}", uuid)
            );
        }
    }
}
