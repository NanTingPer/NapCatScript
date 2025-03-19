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

        var content = new StringContent(msg, enc, contentType);
        HttpResponseMessage hrm = await httpClient.PostAsync(httpUri, content, ctoken);
        return await hrm.Content.ReadAsStringAsync(ctoken);
    }

}
