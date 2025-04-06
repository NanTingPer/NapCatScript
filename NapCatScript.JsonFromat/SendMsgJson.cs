namespace NapCatScript.JsonFromat;

public class SendMsgJson
{
    [JsonIgnore]
    public string JsonText { get => JsonSerializer.Serialize(this);}

    public SendMsgJson(string user_id, List<MsgJson> message, MesgTo sendTo)
    {
        switch (sendTo) {
            case MesgTo.group:
                Group_id = user_id;
                break;
            case MesgTo.user:
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
