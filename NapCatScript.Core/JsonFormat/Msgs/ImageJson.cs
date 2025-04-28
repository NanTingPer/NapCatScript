namespace NapCatScript.Core.JsonFormat.Msgs;

//message
/// <summary>
/// 图片消息的Json对象
/// </summary>
public class ImageJson : MsgJson
{
    public static string ToBase64(string filePath)
    {
        byte[] imageBytes = File.ReadAllBytes(filePath);
        return Convert.ToBase64String(imageBytes);
    }
    public ImageJson(ImageMsgData imageData)
    {
        Data = [imageData];
        JsonText = JsonSerializer.Serialize(this);
    }

    public ImageJson(IEnumerable<ImageMsgData> imageData)
    {
        Data = [];
        foreach (var item in imageData) {
            Data.Add(item);
        }
        JsonText = JsonSerializer.Serialize(this);
    }

    public ImageJson(string imageBase64)
    {
        Data = [];
        Data.Add(new ImageMsgData(imageBase64));
        JsonText = JsonSerializer.Serialize(this);
    }

    [JsonIgnore]
    public override string JsonText { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; } = "image";

    [JsonPropertyName("data")]
    public List<ImageMsgData> Data { get; set; }

    public class ImageMsgData
    {
        public ImageMsgData(string fileBase64)
        {
            File = "base64://" + fileBase64;
        }
        public ImageMsgData(string fileBase64, string summary)
        {
            File = "base64://" + fileBase64;
            Summary = summary;
        }

        [JsonPropertyName("file")]
        public string File { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = "[图片]";
    }
}

