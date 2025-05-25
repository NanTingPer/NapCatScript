using System.Reflection;

namespace NapCatScript.Core.JsonFormat;

public class ArrayMsg
{
    private static List<Type> _types = 
        Assembly
            .GetAssembly(typeof(TextJson))!
            .GetTypes()
            .Where(f => f.Namespace!.StartsWith(typeof(TextJson).Namespace))
            .ToList();
    
    [JsonPropertyName("self_id")]
    public long SelfId { get; set; }

    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("time")]
    public long Time { get; set; }

    [JsonPropertyName("message_id")]
    public long MessageId { get; set; }

    [JsonPropertyName("message_seq")]
    public long MessageSeq { get; set; }

    [JsonPropertyName("real_id")]
    public long RealId { get; set; }

    [JsonPropertyName("real_seq")]
    public string RealSeq { get; set; }

    [JsonPropertyName("message_type")]
    public string MessageType { get; set; }

    [JsonPropertyName("sender")]
    public Sender Sender { get; set; }

    [JsonPropertyName("raw_message")]
    public string RawMessage { get; set; }

    [JsonPropertyName("font")]
    public double FontSize { get; set; }

    [JsonPropertyName("sub_type")]
    public string SubType { get; set; }

    [JsonPropertyName("message")]
    public List<object> Messages { get; set; }

    [JsonPropertyName("message_format")]
    public string MessageFormat { get; set; }

    [JsonPropertyName("post_type")]
    public string PostType { get; set; }

    [JsonPropertyName("group_id")]
    public long? GroupId { get; set; }

    public List<(MsgJson, Type)> GetMsgJsons()
    {
        List<(MsgJson, Type)> list = [];
        
        foreach (var message in Messages) {
            if (!(message is JsonElement element))
                continue;
            element.TryGetPropertyValue("type", out var type);
            string typeStr = type.GetString()!;
            
            foreach (var msgType in _types) {
                if (!msgType.Name.ToLower().Contains(typeStr) && msgType.Name.EndsWith("Json"))
                    continue;

                var msgJson = (MsgJson)element.Deserialize(msgType)!;
                list.Add((msgJson, msgType)); 
                break;
            }
        }

        return list;
    }
    
    /*public MsgType GetMessageType()
    {
        return (MsgType)Enum.Parse(typeof(MsgType), MessageType);
    } */
}

public class Sender
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("nickname")]
    public string NickName { get; set; }

    [JsonPropertyName("card")]
    public string Card { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }
}