using System.Net;
using System.Net.Http;

namespace NapCatScript.Core.MsgHandle;
public static class SendMsg
{
    /// <summary>
    /// 发送消息，返回回应消息
    /// </summary>
    /// <returns></returns>
    public static async Task<string> PostSend(string httpUri, string msg, Encoding? enc, CancellationToken ctoken, Dictionary<string, string>? hands = null, string contentType = "application/json")
    {
        enc ??= Encoding.UTF8;
        var httpClient = new HttpClient();
        if(hands != null) {
            foreach (var hand in hands)
                httpClient.DefaultRequestHeaders.Add(hand.Key, hand.Value);
        }

        try {
            var content = new StringContent(msg, enc, contentType);
            HttpResponseMessage hrm = await httpClient.PostAsync(httpUri, content, ctoken);
            var code = hrm.StatusCode;
            int codein = (int)code;
            if (code == HttpStatusCode.ServiceUnavailable || code == HttpStatusCode.TooManyRequests ||
                codein == 402 || codein == 503 || codein == 500) {
                return "";
            }
            return await hrm.Content.ReadAsStringAsync(ctoken);
        } catch (Exception e) {
            InstanceLog.Erro(e.Message + "\r\n" + e.StackTrace);
            return "Erro";
        }
        
    }

    /// <summary>
    /// 发送消息，返回回应消息
    /// httpURI是请求接口, mesg是ConetntJson
    /// </summary>
    /// <returns></returns>
    public static Task<HttpResponseMessage> PostSend(string httpuri, string msg, Encoding? enc = null, Dictionary<string, string>? hands = null, string contentType = "application/json")
    {
        enc ??= Encoding.UTF8;
        var httpClient = new HttpClient();
        return PostSend(httpClient, httpuri, msg, enc, hands, contentType);
    }

    public static Task<HttpResponseMessage> PostSend(HttpClient httpClient, string httpuri, string msg, Encoding? enc = null, Dictionary<string, string>? hands = null, string contentType = "application/json")
    {
        if (hands != null) {
            foreach (var hand in hands)
                httpClient.DefaultRequestHeaders.Add(hand.Key, hand.Value);
        }

        var content = new StringContent(msg, enc, contentType);
        return httpClient.PostAsync(httpuri, content);
    }
}
