using MesgHandle;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Start
{
    public static class ExtMethod
    {
        public static async Task<string?> ReceiveAsync(this ClientWebSocket socket)
        {
            ArraySegment<byte> bytes = new ArraySegment<byte>(new byte[1024 * 4]);  //创建分片数组
            MemoryStream memResult = new MemoryStream();    //创建内存流
            WebSocketReceiveResult result = await socket.ReceiveAsync(bytes, Main_.CTokrn); //使用分片数组存储消息 每次调用。分片数组的内容会被重置
            do {
                memResult.Write(bytes.Array, bytes.Offset, result.Count); //写入
            } while (!result.EndOfMessage);

            memResult.Seek(0, SeekOrigin.Begin); //头
            StreamReader fStream = new StreamReader(memResult, Encoding.UTF8);
            string mesgString = fStream.ReadToEnd();
            fStream.Dispose();
            fStream.Close();
            memResult.Dispose();
            memResult.Close();
            if(ValidData(mesgString, out var json)) {
                return json?.GetMesgInfo()?.ToString();
            }
            return null;
        }

        /// <summary>
        /// 使用已经过滤的Json主体，获取MesgInfo
        /// </summary>
        public static MesgInfo? GetMesgInfo(this JsonElement json)
        {
            JsonElement user_id;
            JsonElement message;
            JsonElement message_type;

            bool user_id_bool = json.TryGetProperty("user_id", out user_id);
            bool message_bool = json.TryGetProperty("raw_message", out message);
            bool message_type_bool = json.TryGetProperty("message_type", out message_type);

            if (!user_id_bool || !message_bool || !message_type_bool)
                return null;

            return new MesgInfo() { MessageContent = message.GetString(), MessageType = message_type.GetString(), UserId = user_id.GetUInt64() };
        }


        /// <summary>
        /// 判断数据是否是消息，返回json主体
        /// </summary>
        private static bool ValidData(string data, out JsonElement? json)
        {
            Utf8JsonReader read = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(data)));
            //整个json对象
            //JsonDocument jsonObject = JsonSerializer.SerializeToDocument(data); //这样得到的是json字符串
            JsonDocument.TryParseValue(ref read, out JsonDocument? jsonObject);
            JsonElement? jsonRoot = jsonObject?.RootElement;
            json = null;
            try {
                //post_type
                if(jsonRoot is not null) {
                    if (jsonRoot.Value.TryGetProperty("post_type", out JsonElement type)) {//此属性决定是不是消息
                        if (type.ToString() == "message") {
                            json = jsonRoot;
                            return true;
                        }
                        return false;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            } catch (Exception e){
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return false;
            }

        }
    }
    //关于分片数组
    //  1. 指向给定数组的内存
    //  2. 修改分片数组，源值也会更改
    //  3. 分片数组只拥有给定范围
}
