﻿using System;
using System.IO;
using static NapCatScript.JsonFromat.Mesgs.ImageMsgJson;
using static NapCatScript.JsonFromat.Mesgs.TextMsgJson;

namespace NapCatScript.JsonFromat.Mesgs;

public class ImageMsg : BaseMsg
{
    public override string JsonText { get; set; }
    public override JsonElement JsonElement { get; set; }
    public override JsonDocument JsonDocument { get; set; }
    public override dynamic JsonObject { get; set; }

    public ImageMsg(string content)
    {
        var jsonObject = new ImageMsgJson(new ImageMsgData(content));
        JsonText = JsonSerializer.Serialize(jsonObject);
        JsonElement = JsonSerializer.SerializeToElement(jsonObject);
        JsonDocument = JsonSerializer.SerializeToDocument(jsonObject);
        JsonObject = jsonObject;
    }

}

//message
/// <summary>
/// 图片消息的Json对象
/// </summary>
public class ImageMsgJson : MsgJson
{
    public static string ToBase64(string filePath)
    {
        byte[] imageBytes = File.ReadAllBytes(filePath);
        return Convert.ToBase64String(imageBytes);
    }
    public ImageMsgJson(ImageMsgData imageData)
    {
        Data = [imageData];
        JsonText = JsonSerializer.Serialize(this);
    }

    public ImageMsgJson(IEnumerable<ImageMsgData> imageData)
    {
        Data = [];
        foreach (var item in imageData) {
            Data.Add(item);
        }
        JsonText = JsonSerializer.Serialize(this);
    }

    public ImageMsgJson(string imageBase64)
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

