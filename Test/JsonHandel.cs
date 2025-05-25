using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using NapCatScript.Core.JsonFormat;
using NapCatScript.Core.JsonFormat.Msgs;

namespace Test;

public class JsonHandel
{
    
    public static void ArrayMsgParse()
    {
        var jsonObject = JsonSerializer.Deserialize<ArrayMsg>(json);
    }

    public static void ArrayMsgParseMessages()
    {
        var jsonObject = JsonSerializer.Deserialize<ArrayMsg>(json);
        string jsonString = JsonSerializer.Serialize(jsonObject.Messages[0],JsonSerializerOptions.Web);
        jsonString.GetJsonElement(out var jsonElement);
        TextJson tobje = jsonElement.Deserialize<TextJson>()!;
        MsgType.TryParse(typeof(MsgType), tobje.Type, out var msgType);
    }

    public static void ArrayMsgParseToMsgJson()
    {
        var jsonObject = JsonSerializer.Deserialize<ArrayMsg>(json);
        var msgList = jsonObject.GetMsgJsons();
    }
    
    private const string json = """
                                {
                                    "self_id": 235456111,
                                    "user_id": 13246789132,
                                    "time": 564987213,
                                    "message_id": 798546123,
                                    "message_seq": 798546123,
                                    "real_id": 798546123,
                                    "real_seq": "874512",
                                    "message_type": "group",
                                    "sender": {
                                        "user_id": 14146132154,
                                        "nickname": "张三",
                                        "card": "",
                                        "role": "member"
                                    },
                                    "raw_message": "凑人头罢了()",
                                    "font": 14,
                                    "sub_type": "normal",
                                    "message": [
                                        {
                                            "type": "text",
                                            "data": {
                                                "text": "凑人头罢了()"
                                            }
                                        }
                                    ],
                                    "message_format": "array",
                                    "post_type": "message",
                                    "group_id": 8745123456781
                                }
                                """;
}