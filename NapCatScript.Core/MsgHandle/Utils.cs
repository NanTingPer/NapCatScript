using NapCatScript.Core.Model;
using System.Net.Http.Headers;
using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Core.Services;
using HttpClient = System.Net.Http.HttpClient;

namespace NapCatScript.Core.MsgHandle;
/// <summary>
/// /api/OB11Config/SetConfig => 设置网络配置 需要 authorization Post
/// 
/// </summary>
public static class Utils
{
    public const string WEBUILOGINGAPI = "/api/auth/login";
    public const string WEBUILOGAPI = "/api/Log/GetLogRealTime";
    public const string WEBUIGETCONFIG = "/api/OB11Config/GetConfig";
    public const string WEBUISETCONFIG = "/api/OB11Config/SetConfig";

    #region WebUi
    
    /// <summary>
    /// 获取网络配置
    /// </summary>
    /// <param name="httpUrl"> WebUI的URL ： http://127.0.0.1:6099 </param>
    /// <param name="aut"> <see cref="GetAuthentication(string,string)"/> </param>
    /// <returns></returns>
    public static async Task<string?> GetWebUiNetWorkConfig(string httpUrl, string aut)
    {
        httpUrl = httpUrl.DelEnd() + WEBUIGETCONFIG;
        
        var hc = new HttpClient();
        hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", aut);
        var result = await hc.PostAsync(httpUrl, null);
        try {
            return await result.Content.ReadAsStringAsync();
        }
        catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
            return null;
        }
    }
    
    #endregion
    
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
        if (mesg.GroupId.ToString() != string.Empty &&
            mesg.GroupId != default)
            return mesg.GroupId.ToString();
        return mesg.UserId.ToString();
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
    public static Task<string> GetAuthentication(string httpUri, string webport, string token)
    {
        string httpuri = string.Join(":", httpUri.Split(":")[..2]) + $":{webport}";
        return GetAuthentication(httpuri, token);
    }

    public static async Task<string> GetAuthentication(string httpUri, string token)
    {
        var r = await WebUILogin(httpUri, token);
        return await GetAuthentication(r);
    }

    /// <summary>
    /// 使用<see cref="WebUILogin"/>的HttpResponseMessage
    /// </summary>
    /// <param name="hrm"></param>
    /// <returns></returns>
    public static async Task<string> GetAuthentication(HttpResponseMessage hrm)
    {
        string con = await hrm.Content.ReadAsStringAsync();
        if (con.GetJsonElement(out var je)) {
            if (je.TryGetPropertyValue("Credential", out je)) {
                return je.GetString()!;
            }
        }
        return "";
    }
    
    /// <summary>
    /// 获取登录WebUI的返回,message="success"则成功，code=0
    /// </summary>
    /// <param name="httpUri"> http://127.0.0.1:8888/ or http://127.0.0.1:8888 </param>
    /// <param name="token"> webUi登录密钥 </param>
    /// <returns></returns>
    public static Task<HttpResponseMessage> WebUILogin(string httpUri, string token)
    {
        httpUri = httpUri.DelEnd();
        var requUri = httpUri + WEBUILOGINGAPI;
        string json = $$"""{"token":"{{token}}"}""";
        HttpClient client = new HttpClient();
        return client.PostAsync(requUri, new StringContent(json, Encoding.UTF8, "application/json"));
    }
    
    /// <summary>
    /// 获取WebUI的Log
    /// <para> 需要端口的原因是因为在Core中的公共配置含有HTTP配置 </para>
    /// </summary>
    /// <param name="httpUri"> WebUI的连接地址 : http://127.0.0.1:9999 </param>
    /// <param name="webPort"> WebUI的连接端口: 6099 </param>
    /// <param name="webToken"> WebUI的登录Token : napcat </param>
    /// <returns></returns>
    public static async IAsyncEnumerable<string?> GetWebUILog(string httpUri, string webPort, string webToken)
    {
        string httpuri = string.Join(":", httpUri.Split(":")[..2]) + $":{webPort}";
        string aut = await GetAuthentication(httpuri, webPort, webToken);
        await foreach (var se in GetWebUILog(httpuri, aut))
        {
            yield return se;
        }
    }

    /// <summary>
    /// 获取WebUI的Log
    /// </summary>
    /// <param name="httpUri"> 用于登录WebUI的链接 : http://127.0.0.1:6099 </param>
    /// <param name="aut"> 密钥 可以使用<see cref="GetAuthentication(string,string)"/> </param>
    public static async IAsyncEnumerable<string?> GetWebUILog(string httpUri, string aut)
    {
        string rquUri = httpUri + WEBUILOGAPI;
        var hc = new HttpClient();
        hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", aut);
        var r = await hc.GetAsync(rquUri, HttpCompletionOption.ResponseHeadersRead);
        var stream = new StreamReader(r.Content.ReadAsStream());
        string? getStr;
        while (!stream.EndOfStream) {
            getStr = stream.ReadLine();
            yield return getStr;
        }
    }

    
    private static string DelEnd(this string url)
    {
        if (url.EndsWith('/')) {
            return url.Substring(0, url.Length - 1);
        }
        return url;
    }
    private static string JoinUrlProtApi(this string url, string port, string api)
    {
        string httpuri = string.Join(":", url.Split(":")[..2]) + $":{port}";
        return httpuri + api;
    }

    public static string JoinUrlProtAPI(string url, string port, string api) => url.JoinUrlProtApi(port, api);
}