namespace NapCatScript.Core.JsonFormat.GetJsons;

public class get_image
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("retcode")]
    public double Retcode { get; set; }

    [JsonPropertyName("data")]
    public ImageInfo ImageInfoData { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("wording")]
    public string Wording { get; set; }

    [JsonPropertyName("echo")]
    public string? Echo { get; set; }
}

public class ImageInfo
{
    [JsonPropertyName("file")]
    public string LocalPath { get; set; }

    [JsonPropertyName("url")]
    public string NetWorkPath { get; set; }

    [JsonPropertyName("file_size")]
    public string FileSize { get; set; }

    [JsonPropertyName("file_name")]
    public string FileName { get; set; }

    [JsonPropertyName("base64")]
    public string Base64 { get; set; }
}