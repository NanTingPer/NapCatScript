namespace NapCatScript.JsonFromat;

public class SendJson
{
    [JsonIgnore]
    public string JsonText { get => JsonSerializer.Serialize(this);}

    public SendJson(string user_id, List<MsgJson> message, MsgTo sendTo)
    {
        switch (sendTo) {
            case MsgTo.group:
                Group_id = user_id;
                break;
            case MsgTo.user:
                User_id = user_id;
                break;
        }
        Message = message;
    }

    [JsonPropertyName("user_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? User_id { get; set; } = null;

    [JsonPropertyName("group_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Group_id { get; set; } = null;

    [JsonPropertyName("message")]
    public List<MsgJson> Message { get; set; }
}
