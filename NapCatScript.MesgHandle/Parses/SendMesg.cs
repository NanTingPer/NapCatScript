using System.Net;
using System.Net.Http;

namespace NapCatScript.MesgHandle.Parses;
public static class SendMesg
{
    /// <summary>
    /// 发送消息，返回回应消息
    /// </summary>
    /// <returns></returns>
    public static async Task<string> Send(string httpUri, string msg, Encoding? enc, CancellationToken ctoken, Dictionary<string, string>? hands = null, string contentType = "application/json")
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
            Loging.Log.Erro(e.Message + "\r\n" + e.StackTrace);
            return "Erro";
        }
        
    }

}
