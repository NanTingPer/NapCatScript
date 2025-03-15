using NapCatScript.JsonFromat;

namespace NapCatScript.MesgHandle;
public static class Utils
{
    ///<summary>
    ///获取Msg消息URL 基本消息
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
}

