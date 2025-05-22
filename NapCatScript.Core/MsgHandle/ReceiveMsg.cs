using NapCatScript.Core.JsonFormat.EventJson;
using NapCatScript.Core.Model;
using System.Buffers;

namespace NapCatScript.Core.MsgHandle;
public static class ReceiveMsg
{
    public static async Task<(MsgInfo?, string)?> ReceiveMsgInfo(this ClientWebSocket socket)
    {
        if (socket.State != WebSocketState.Open)
            return null;

        ArraySegment<byte>? bytes = null;
        MemoryStream? memResult = null;
        WebSocketReceiveResult? result = null;
        try {
            bytes = new ArraySegment<byte>(new byte[1024 * 200]);  //创建分片数组
            memResult = new MemoryStream();    //创建内存流
            result = await socket.ReceiveAsync(bytes.Value, CancellationToken.None); //使用分片数组存储消息 每次调用。分片数组的内容会被重置
            do {
                memResult.Write(bytes.Value.Array, bytes.Value.Offset, result.Count); //写入
            } while (!result.EndOfMessage);

            memResult.Seek(0, SeekOrigin.Begin); //头
            StreamReader fStream = new StreamReader(memResult, Encoding.UTF8);
            string mesgString = fStream.ReadToEnd();
            fStream.Dispose();
            fStream.Close();
            memResult.Dispose();
            memResult.Close();
            if (ValidData(mesgString, out var json)) {
                return (json?.GetMesgInfo(), mesgString)/*?.ToString()*/;
            }
            return null;
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
            if(memResult is not null) {
                memResult.Dispose();
                memResult.Close();
            }
            return null;
        }

    }
    
    public static async Task<MsgInfo?> Receive(this ClientWebSocket socket, CancellationToken CToken)
    {
        (MsgInfo? msgInfo, string msgString)? msg = await socket.ReceiveMsgInfo();
        if(msg is null)
            return null;
        
        return msg.Value.msgInfo;
    }

    public static async Task<(MsgInfo?, ArrayMsg?)?> ReceiveMsgInfoAndJson(this ClientWebSocket socket)
    {
        (MsgInfo? msgInfo, string msgString)? info = await socket.ReceiveMsgInfo();
        if (info is null)
            return null;

        var value = info.Value;
        
        if(value.msgInfo is null) return null;
        ArrayMsg? msg;
        try {
            msg = JsonSerializer.Deserialize<ArrayMsg>(value.msgString);
        }
        catch (Exception e) {
            msg = null;
        }
        
        return (value.msgInfo, msg);
    }
    
    /// <summary>
    /// 使用已经过滤的Json主体，获取MesgInfo
    /// </summary>
    private static MsgInfo? GetMesgInfo(this JsonElement json)
    {
        bool message_type_bool = json.TryGetProperty("message_type", out JsonElement message_type);
        JsonElement user_id = new JsonElement();

        bool user_id_bool = json.TryGetProperty("user_id", out user_id);
        bool group_id_bool = json.TryGetProperty("group_id", out JsonElement group_id);
        bool message_bool = json.TryGetProperty("raw_message", out JsonElement message);

        if (!user_id_bool || !message_bool || !message_type_bool)
            return null;

        string? user_name = "";
        if (json.TryGetProperty("sender", out JsonElement sender)) {
            if (sender.TryGetProperty("nickname", out JsonElement value))
                user_name = value.GetString();
        }

        JsonElement time;
        double d1 = 0d;
        if (json.TryGetProperty("time", out time)) {
            d1 = time.GetDouble();
        }
        
        JsonElement message_id;
        long msgid = 0L;
        if (json.TryGetProperty("message_id", out message_id)) {
            msgid = message_id.GetInt64();
        }
        
        return new MsgInfo()
        {
            MessageContent = message.GetString()!, 
            MessageType = message_type.GetString()!, 
            UserId = user_id.GetUInt64().ToString(), 
            GroupId = group_id_bool ? group_id.GetInt64().ToString() : /*default*/string.Empty, 
            UserName = user_name ?? "",
            Time = d1,
            MessageId = msgid
        };
    }


    /// <summary>
    /// 判断数据是否是消息，返回json主体
    /// </summary>
    private static bool ValidData(string data, out JsonElement? json)
    {
        json = null;
        Utf8JsonReader read = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(data)));
        //整个json对象
        //JsonDocument jsonObject = JsonSerializer.SerializeToDocument(data); //这样得到的是json字符串
        JsonDocument.TryParseValue(ref read, out JsonDocument? jsonObject);
        JsonElement? jsonRoot = jsonObject?.RootElement;
        try {
            //post_type
            if (jsonRoot is not null) {
                if (jsonRoot.Value.TryGetProperty("post_type", out JsonElement type)) {//此属性决定是不是消息
                    if (type.ToString() == "message") {
                        json = jsonRoot;
                        return true;
                    }

                    //上报自身消息
                    if(type.ToString() == "message_sent") {
                        json = jsonRoot;
                        return true;
                    }
                    return false;
                } else 
                    return false;
            } else 
                return false;
        } catch (Exception e) {
            Console.WriteLine(e.Message + "\n" + e.StackTrace);
            return false;
        }

    }
    //关于分片数组
    //  1. 指向给定数组的内存
    //  2. 修改分片数组，源值也会更改
    //  3. 分片数组只拥有给定范围
}

