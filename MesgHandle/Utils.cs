using NapCatScript.JsonFromat;
using NapCatScript.JsonFromat.Mesgs;
using NapCatScript.MesgHandle.Parses;

namespace NapCatScript.MesgHandle;
public static class Utils
{
    ///<summary>
    ///获取Msg消息URL 基本消息(API访问链接)
    ///<para> uri是原始链接 例如: http://127.0.0.1:6666 </para>
    ///</summary>
    public static string GetMsgURL(string uri, MesgInfo message, out MesgTo mesg)
    {
        if (message.MessageType == "group") {
            mesg = MesgTo.group;
            if (uri.EndsWith("/")) return uri + API.GroupMsgNoX;
            else return uri + API.GroupMsg;
        }

        if (message.MessageType == "private") {
            mesg = MesgTo.user;
            if (uri.EndsWith("/")) return uri + API.PrivateMsgNoX;
            else return uri + API.PrivateMsg;
        }
        mesg = MesgTo.user;
        return "";
    }

    /// <summary>
    /// 获取目标ID
    /// <para>如果是群消息，返回群ID</para>
    /// </summary>
    public static string GetUserId(MesgInfo mesg)
    {
        if (mesg.GroupId != 0)
            return mesg.GroupId.ToString();
        return mesg.UserId.ToString();
    }

    /// <summary>
    /// 发生一个给定的普通文本
    /// </summary>
    /// <param name="mesg"> 消息引用 </param>
    /// <param name="httpURI"> 目标基础uri 例: http://127.0.0.1:6666 </param>
    /// <param name="content"> 消息内容 </param>
    /// <returns></returns>
    public static async void SendTextAsync(MesgInfo mesg, string httpURI, string content, CancellationToken ct)
    {
        string sendUri = GetMsgURL(httpURI, mesg, out var mesgType);
        TextMesg r = new TextMesg(GetUserId(mesg), mesgType, content);
        string conet = r.MesgString;
        await SendMesg.Send(sendUri, conet, null, ct);
    }
}

