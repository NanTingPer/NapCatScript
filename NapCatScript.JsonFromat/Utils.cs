using System;
using System.IO;
using System.Text.Json;

namespace NapCatScript.JsonFromat;

public static class Utils
{
    #region JsonDocument扩展方法
    /// <summary>
    /// 获取JsonDocument的Root对象
    /// </summary>
    public static JsonElement Root(this JsonDocument jd)
    {
        return jd.RootElement;
    }

    #endregion

    public static string ImageToBase64(string filePath)
    {
        byte[] imageBytes = File.ReadAllBytes(filePath);
        return "base64://" + Convert.ToBase64String(imageBytes);
    }
}
