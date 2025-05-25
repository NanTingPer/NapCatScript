namespace NapCatScript.Core.JsonFormat;

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

    public static bool GetJsonElement(this string str, out JsonElement element)
    {
        element = new JsonElement();
        try
        {
            var d = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(str)));
            if(JsonDocument.TryParseValue(ref d, out JsonDocument? jsonDoc)) {
                element = jsonDoc.RootElement;
                return true;
            }
        } catch (Exception e) {
            return false;
        }
        return false;
    }
    public static bool TryGetPropertyValue(this JsonElement element, string propertyname, out JsonElement rvalue)
    {
        List<JsonElement> stack = [element];
        while(stack.Count > 0) {
            JsonElement je = stack[0];
            if(je.ValueKind == JsonValueKind.Object) {
                if(je.TryGetProperty(propertyname, out rvalue)) {
                    return true;
                }
                foreach (JsonProperty item in je.EnumerateObject().AsEnumerable()) {
                    stack.Add(item.Value);
                }
            } else if(je.ValueKind == JsonValueKind.Array) {
                foreach (var item in je.EnumerateArray().AsEnumerable()) {
                    stack.Add(item);
                }
            }
            stack.RemoveAt(0);
        }
        rvalue = new JsonElement();
        return false;
    }
}
