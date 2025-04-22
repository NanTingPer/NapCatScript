using NapCatScript.Core.JsonFormat.EventJson;
using NapCatScript.Core.Model;
using System.Buffers;

namespace NapCatScript.Core.MsgHandle;
public static class ReceiveMesg
{
    public static async Task<MsgInfo?> Receive(this ClientWebSocket socket, CancellationToken CToken)
    {
        if (socket.State != WebSocketState.Open)
            return null;

        ArraySegment<byte>? bytes = null;
        MemoryStream? memResult = null;
        WebSocketReceiveResult? result = null;
        try {
            bytes = new ArraySegment<byte>(new byte[1024 * 200]);  //创建分片数组
            memResult = new MemoryStream();    //创建内存流
            result = await socket.ReceiveAsync(bytes.Value, CToken); //使用分片数组存储消息 每次调用。分片数组的内容会被重置
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

            #region 心跳
            //try {
            //    if (lifeCycle != null) {
            //        var msg = new MesgInfo();
            //        if (!long.TryParse(lifeCycle.Time, out long lifetime))
            //            return null;
            //        msg.lifeTime = lifetime;
            //        return msg;
            //    }
            //} catch (Exception e) {
            //    Log.Erro(e.Message, e.StackTrace);
            //}
            #endregion


            if (ValidData(mesgString, out var json, out long time)) {
                if(time == 0 && json != null)
                    return json?.GetMesgInfo()/*?.ToString()*/;
                if (time != 0 && json == null) {
                    MsgInfo mesg = new MsgInfo();
                    mesg.lifeTime = time;
                    return mesg;
                }
                if (time != 0 && json != null) {
                    MsgInfo mesg = json?.GetMesgInfo()!;
                    mesg!.lifeTime = time;
                    return mesg;
                }
            }
            return null;
        } catch (Exception e) {
            Log.Erro(e.Message, e.StackTrace);
            if(memResult is not null) {
                memResult.Dispose();
                memResult.Close();
            }
            return null;
        }
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
        return new MsgInfo() { MessageContent = message.GetString()!, MessageType = message_type.GetString()!, UserId = user_id.GetUInt64().ToString(), GroupId = group_id_bool ? group_id.GetUInt64().ToString() : string.Empty, UserName = user_name is null ? "" : user_name };
    }


    /// <summary>
    /// 判断数据是否是消息，返回json主体
    /// </summary>
    private static bool ValidData(string data, out JsonElement? json, out long time)
    {
        json = null;
        time = 0;
        Utf8JsonReader read = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(data)));
        //整个json对象
        //JsonDocument jsonObject = JsonSerializer.SerializeToDocument(data); //这样得到的是json字符串
        JsonDocument.TryParseValue(ref read, out JsonDocument? jsonObject);
        JsonElement? jsonRoot = jsonObject?.RootElement;
        try {
            //post_type
            if (jsonRoot is not null) {
                if (jsonRoot.Value.TryGetProperty("time", out var value))
                    if (value.TryGetInt64(out time)) { }
                if (jsonRoot.Value.TryGetProperty("post_type", out JsonElement type)) {//此属性决定是不是消息
                    if (type.ToString() == "message") {
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

