using NapCatScript.Core.Model;

namespace NapCatScript.Core.MsgHandle;
public static class Utils
{
    ///<summary>
    ///获取Msg消息URL 基本消息(API访问链接)
    ///<para> uri是原始链接 例如: http://127.0.0.1:6666 </para>
    ///</summary>
    public static string GetMsgToURL(this MesgInfo message, string uri)
    {
        if (message.MessageType == "group") {
            if (uri.EndsWith('/')) return uri + API.GroupMsgNoX;
            else return uri + API.GroupMsg;
        }

        if (message.MessageType == "private") {
            if (uri.EndsWith('/')) return uri + API.PrivateMsgNoX;
            else return uri + API.PrivateMsg;
        }
        return "";
    }

    /// <summary>
    /// 获取目标ID
    /// <para>如果是群消息，返回群ID</para>
    /// </summary>
    public static string GetUserId(MesgInfo mesg)
    {
        if (mesg.GroupId != string.Empty)
            return mesg.GroupId;
        return mesg.UserId;
    }

    public static MsgTo GetMsgTo(this MesgInfo info)
    {
        if (info.MessageType == "group")
            return MsgTo.group;
        else
            return MsgTo.user;
    }

    /// <summary>
    /// 利用MesgInfo的内容，将文本发送出去
    /// </summary>
    /// <param name="mesg"> 消息引用 </param>
    /// <param name="httpURI"> 目标基础uri 例: http://127.0.0.1:6666 </param>
    /// <param name="content"> 消息内容 </param>
    /// <returns></returns>
    public static async void SendTextAsync(MesgInfo mesg, string httpURI, string content, CancellationToken ct)
    {
        string sendUri = mesg.GetMsgToURL(httpURI);
        TextMesg r = new TextMesg(GetUserId(mesg), mesg.GetMsgTo(), content);
        string conet = r.MesgString;
        await SendMesg.Send(sendUri, conet, null, ct);
    }
}