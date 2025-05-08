using NapCatScript.Core.Model;
using System.Net.Http.Headers;

namespace NapCatScript.Core.MsgHandle;
/// <summary>
/// /api/OB11Config/SetConfig => 设置网络配置 需要 authorization Post
/// 
/// </summary>
public static class Utils
{
    ///<summary>
    ///获取Msg消息URL 基本消息(API访问链接)
    ///<para> uri是原始链接 例如: http://127.0.0.1:6666 </para>
    ///</summary>
    public static string GetMsgToURL(this MsgInfo message, string uri)
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
    public static string GetUserId(MsgInfo mesg)
    {
        if (mesg.GroupId != string.Empty)
            return mesg.GroupId;
        return mesg.UserId;
    }

    public static MsgTo GetMsgTo(this MsgInfo info)
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
    public static async void SendTextAsync(MsgInfo mesg, string httpURI, string content, CancellationToken ct)
    {
        string sendUri = mesg.GetMsgToURL(httpURI);
        TextMesg r = new TextMesg(GetUserId(mesg), mesg.GetMsgTo(), content);
        string conet = r.MesgString;
        await SendMsg.PostSend(sendUri, conet, null, ct);
    }

    private static async Task<string> GetClientkey(string httpUri)
    {
        var cookies = await SendMsg.PostSend(httpUri + "/get_clientkey", "");
        return await cookies.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 获取部分api需要的Authentication
    /// </summary>
    /// <param name="httpUri"> 例: http://127.0.0.1:9999 </param>
    /// <param name="webport"> napcat的web端口 </param>
    /// <param name="token"> web登录密码 </param>
    /// <returns></returns>
    public static async Task<string> GetAuthentication(string httpUri, string webport, string token)
    {
        string httpuri = string.Join(":", httpUri.Split(":")[..2]) + $":{webport}";
        string uri = httpuri + "/api/auth/login";
        string json = $$"""{"token":"{{token}}"}""";
        HttpClient client = new HttpClient();
        var r = await client.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
        string con = await r.Content.ReadAsStringAsync();
        if (con.GetJsonElement(out var je)) {
            if (je.TryGetPropertyValue("Credential", out je)) {
                return je.GetString()!;
            }
        }
        return "";
    }

    private static async Task GetLoging(string authentication)
    {
        var hc = new HttpClient();
        hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authentication);
        var r = await hc.GetAsync("http://127.0.0.1:6099/api/Log/GetLogRealTime", HttpCompletionOption.ResponseHeadersRead);
        var stream = new StreamReader(r.Content.ReadAsStream());
        string? getStr;
        while (!stream.EndOfStream) {
            getStr = stream.ReadLine();
            Console.WriteLine(getStr);
        }
    }

}