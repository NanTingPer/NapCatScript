namespace NapCatScript.JsonFromat.Mesgs;

public class TextMesgBak : BaseMesg
{
    public override string JsonText { get; set; }
    public override JsonElement JsonElement { get; set; }
    public override JsonDocument JsonDocument { get; set; }
    public override dynamic JsonObject { get; set; }

    public TextMesgBak(string content)
    {
        var jsonObject = new Root(new Data(content));
        JsonText = JsonSerializer.Serialize(jsonObject);
        JsonElement = JsonSerializer.SerializeToElement(jsonObject);
        JsonDocument = JsonSerializer.SerializeToDocument(jsonObject);
        JsonObject = jsonObject;
    }

    public class Root
    {
        public Root(Data data, string type = "text")
        {
            Data = data;
            Type = type;
        }

        [JsonPropertyName("type")]
        public string Type { get; set; } = "text";

        [JsonPropertyName("data")]
        public Data Data { get; set; } = new Data();
    }

    public class Data
    {
        public Data(string text = "unll")
        {
            Text = text;
        }

        [JsonPropertyName("text")]
        public string Text { get; set; } = "null";
    }
}
