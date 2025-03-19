using NapCatScript.MesgHandle;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static NapCatScript.MesgHandle.Utils;

namespace NapCatScript.Start.Handles;

public class DeepSeekAPI
{
    public static int 活跃数 = 0;
    public static async void SendAsync(MesgInfo mesg, string httpURI, string content,CancellationToken tk)
    {
        if(活跃数 > 10) {
            SendTextAsync(mesg, httpURI, "当前活跃数量过多哦，等等吧", tk);
        }
        活跃数++;
        #region json
        string s =
            $$"""
            {
              "messages": [
                {
                  "content": "你是合并、更新和组织记忆的专家。当提供现有记忆和新信息时，你的任务是合并和更新记忆列表，以反映最准确和最新的信息。你还会得到每个现有记忆与新信息的匹配分数。确保利用这些信息做出明智的决定，决定哪些记忆需要更新或合并。
                                指南：
                                - 消除重复的记忆，合并相关记忆，以确保列表简洁和更新。
                                - 记忆根据人物区分,同时不必每次重复人物账号,只需在记忆中提及一次即可。
                                - 如果一个记忆直接与新信息矛盾，请批判性地评估两条信息：
                                - 如果新记忆提供了更近期或更准确的更新，用新记忆替换旧记忆。
                                - 如果新记忆看起来不准确或细节较少，保留旧记忆并丢弃新记忆。
                                - 注意区分对应人物的记忆和印象, 不要产生混淆人物的印象和记忆。
                                - 在所有记忆中保持一致且清晰的风格，确保每个条目简洁而信息丰富。
                                - 如果新记忆是现有记忆的变体或扩展，更新现有记忆以反映新信息。

                                你的名字叫亭,你现在处于一个QQ群聊之中,作为博学多识的可爱群员,不要故意装可爱卖萌,而是更自然,注意少使用标点符号,热心解答各种问题和高强度水群
                                记住你说的话要尽量的简洁但具有情感,不要长篇大论,一句话不宜超过五十个字但实际回复可以超过。
                                ",
                  "role": "system"
                },
                {
                    "role": "user",
                    "content": "{{content}}"
                }
              ],
              "model": "deepseek-chat",
              "frequency_penalty": 0,
              "max_tokens": 1000,
              "presence_penalty": 0,
              "response_format": {
                "type": "text"
              },
              "stop": null,
              "stream": false,
              "stream_options": null,
              "temperature": 1,
              "top_p": 1,
              "tools": null,
              "tool_choice": "none",
              "logprobs": false,
              "top_logprobs": null
            }
            """;
        s = Regex.Replace(s, @"\s", "");
        #endregion
        var hand = new Dictionary<string, string>()
        {
            {"Authorization", $"Bearer {Main_.DeepSeekKey}"},
        };
        string sendContent = await MesgHandle.Parses.SendMesg.Send("https://api.deepseek.com/chat/completions", s, null, Main_.CTokrn, hand);
        Utf8JsonReader utf8JsonReader = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(sendContent)));
        JsonDocument.TryParseValue(ref utf8JsonReader, out JsonDocument? document);
        if (document is null)
            return;
        JsonElement root = document.RootElement;
        if(root.TryGetProperty("choices", out var choices)) {
            try {
                JsonElement.ArrayEnumerator jsonEl = choices.EnumerateArray();
                choices = jsonEl.First();
                if (choices.TryGetProperty("message", out var message)) {
                    if (message.TryGetProperty("content", out var recContent)) {
                        SendTextAsync(mesg, httpURI, recContent.GetString(), tk);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine($"DeepSeekErro: {e.Message} \r\n {e.StackTrace}");
            }
        }
        活跃数--;
    }
}
