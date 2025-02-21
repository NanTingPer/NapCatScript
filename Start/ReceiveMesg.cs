using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Start
{
    public static class ExtMethod
    {
        public static async Task<string> ReceiveAsync(this ClientWebSocket socket)
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
            return mesgString;
        }
    }
    //关于分片数组
    //  1. 指向给定数组的内存
    //  2. 修改分片数组，源值也会更改
    //  3. 分片数组只拥有给定范围
}
