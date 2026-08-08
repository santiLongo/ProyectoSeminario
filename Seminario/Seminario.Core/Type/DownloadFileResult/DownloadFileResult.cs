namespace Seminario.Core.Type.DownloadFileResult;

public class DownloadFileResult
{
    public byte[] Bytes { get; set; } = [];
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/octet-stream";
}