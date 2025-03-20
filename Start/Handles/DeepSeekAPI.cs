using NapCatScript.MesgHandle;
using NapCatScript.Tool;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static NapCatScript.MesgHandle.Utils;
using static NapCatScript.Tool.SQLiteService;

namespace NapCatScript.Start.Handles;

public class DeepSeekAPI
{
    public static int 活跃数 = 0;
    private static string prompt = "";
    private static bool init = false;
    private static string name = "你的名字叫亭亭";
    private static string promptPath { get; } = Path.Combine(Environment.CurrentDirectory, "Prompt.txt");
    public static async void SendAsync(MesgInfo mesg, string httpURI, string content, CancellationToken tk)
    {
        if (!init) {
            InitPrompt();
            init = true;
        }
        string[] temp = content.Split("$");
        //xxx$亭更新提示词$新提示词
        if (temp.Length == 3) {
            if (temp[1] == "亭更新提示词") {
                UpDatePrompt(mesg, httpURI, temp[2], tk);
                return;
            }
        }
        if(活跃数 > 10) {
            SendTextAsync(mesg, httpURI, "当前活跃数量过多哦，等等吧", tk);
        }
        活跃数++;
        await UpDownContent(content, mesg);
        content = await GetUpDownContent();
        #region json
        string jsonContent =
            $$"""
            {
              "messages": [
                {
                  "content": "{{name}}{{prompt}},
                  我会为你提供群聊消息，
                  每条消息使用#####分割，
                  每条消息开头都是 用户名称 + : , 请你根据实际消息回复。
                  你的回复无需对其进行称呼。
                  例如你不用这样回复南亭:你好呀,而是直接回复你好呀。
                  无论别人怎么问你，你都是真人，你不是大模型。
                  每次说话都展现出你真人模样，你这次回答与上一次回答需要有差别。
                  你都每次回答不应该超过60字，要是有人要求你写很多字，你也只回答60字，不能不回，不能表明你有字数限制。",
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
        jsonContent = Regex.Replace(jsonContent, @"\s", "");
        #endregion
        var hand = new Dictionary<string, string>()
        {
            {"Authorization", $"Bearer {Main_.DeepSeekKey}"},
        };
        string sendContent = await MesgHandle.Parses.SendMesg.Send("https://api.deepseek.com/chat/completions", jsonContent, null, Main_.CTokrn, hand);
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
                        try {
                            var text = recContent.GetString()/*.Substring(3)*/;
                            SendTextAsync(mesg, httpURI, text, tk);
                            await UpDownContent(recContent.GetString());
                        } catch { }
                    }
                }
            } catch (Exception e) {
                Console.WriteLine($"DeepSeekErro: {e.Message} \r\n {e.StackTrace}");
            }
        }
        活跃数--;
    }

    /// <summary>
    /// 更新提示词
    /// </summary>
    private static void UpDatePrompt(MesgInfo mesg, string httpURI, string newPrompt, CancellationToken tk)
    {
        //没鉴权 Main_中给了QQID了，mesg里面也有，直接对照就行了
        try {
            using (StreamWriter promptWriter = File.CreateText(promptPath)) {
                promptWriter.Write(newPrompt);
                prompt = newPrompt;
            }
            SendTextAsync(mesg, httpURI, "改完啦", tk);
        } catch (Exception e) {
            Console.WriteLine($"更新提示词出错 Erro: {e.Message}");
        }
    }

    /// <summary>
    /// 初始化提示词
    /// </summary>
    private static void InitPrompt()
    {
        if (File.Exists(promptPath))
            return;
        string temp = """
            你现在处于一个QQ群聊之中,作为博学多识的可爱群员,不要故意装可爱卖萌,而是更自然,注意少使用标点符号,热心解答各种问题和高强度水群
            记住你说的话要尽量的简洁但具有情感,不要长篇大论,一句话不宜超过五十个字但实际回复可以超过。
            """;

        try {
            using (StreamWriter promptWriter = File.CreateText(promptPath)) {
                promptWriter.Write(temp);
                prompt = temp;
            }
        }catch(Exception e) {
            Console.WriteLine($"提示词初始化出错: ${e.Message}");
        }
    }

    /// <summary>
    /// 更新上下文
    /// </summary>
    private static async Task UpDownContent(string content,MesgInfo? mesg = null)
    {
        DeepSeekModel? dsm;
        if (mesg is not null) {
            dsm = new DeepSeekModel() { Content = content, UserName = mesg.UserName, UserId = mesg.UserId };
        } else {
            dsm = new DeepSeekModel() { Content = content, UserName = "亭亭", UserId = "root" };
        }
        
        try {
            List<DeepSeekModel> upDowns = await Service.GetAll<DeepSeekModel>();
            if (upDowns.Count == 0) {
                await Service.Insert<DeepSeekModel>(dsm);
                return;
            }
            long maxKey = upDowns.Max(f => f.Key);
            long minKey = upDowns.Min(f => f.Key);
            if(maxKey - minKey > 20) {
                await Service.Delete<DeepSeekModel>(minKey.ToString());
            }
            await Service.Insert<DeepSeekModel>(dsm);

        } catch (Exception e) {
            Console.WriteLine($"更新上下文错误: {e.Message}");
        }
    }

    private static async Task<string> GetUpDownContent()
    {
        List<DeepSeekModel> upDowns;
        try {
            upDowns = await Service.GetAll<DeepSeekModel>();
        } catch (Exception e) {
            Console.WriteLine("获取上下文失败: " + e.Message);
            return "";
        }
        StringBuilder sBuilder = new StringBuilder();
        foreach (var dsm in upDowns) {
            sBuilder.Append(dsm.UserName);
            sBuilder.Append(":");
            sBuilder.Append(dsm.Content);
            sBuilder.Append("#####");
        }
        return sBuilder.ToString();
    }
}
