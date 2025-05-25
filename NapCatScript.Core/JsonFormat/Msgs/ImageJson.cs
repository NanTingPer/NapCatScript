using System.Diagnostics.CodeAnalysis;

namespace NapCatScript.Core.JsonFormat.Msgs;

//message
/// <summary>
/// 图片消息的Json对象
/// </summary>
public class ImageJson : MsgJson
{
    #region ctor

    [JsonConstructor]
    private ImageJson()
    {
        
    }

    public ImageJson(ImageJsonData imageData)
    {
        Data = imageData;
    }

    public ImageJson(string imageBase64)
    {
        Data = new ImageJsonData(imageBase64);
    }


    #endregion

    /// <summary>
    /// 使用文件路径创建ImageJson
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static ImageJson Create(string filePath) => new ImageJson(ImageToBase64(filePath));
    
    public static string ToBase64(string filePath)
    {
        byte[] imageBytes = File.ReadAllBytes(filePath);
        return Convert.ToBase64String(imageBytes);
    }

    [JsonPropertyName("type")]
    public string Type { get; } = "image";

    [JsonPropertyName("data")]
    public ImageJsonData Data { get; set; }

    public class ImageJsonData
    {
        [JsonConstructor]
        private ImageJsonData()
        {
            
        }
        
        public ImageJsonData(string fileBase64)
        {
            File = "base64://" + fileBase64;
        }
        
        public ImageJsonData(string fileBase64, string summary)
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

