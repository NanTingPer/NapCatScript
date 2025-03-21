using NapCatScript.MesgHandle;
using NapCatScript.Tool;
using System.Security.Cryptography;
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
        GoTo @goto = await Command(mesg, temp, httpURI, tk);
        switch (@goto) {
            case GoTo.None:
                return;
            case GoTo.GoOn:
                await UpDownContent(content, mesg, 1);
                content = await GetUpDownContent<DeepSeekModel>(mesg);
                break;
            case GoTo.Con:
                content = await GetUpDownContent<DeepSeekGroupModel>(mesg);
                content += $"#####{content}";
                break;
            default:
                content = await GetUpDownContent<DeepSeekModel>(mesg);
                break;
        }

        if (活跃数 > 11) {
            SendTextAsync(mesg, httpURI, "当前活跃数量过多哦，等等吧", tk);
        }
        活跃数++;
        #region json
        string jsonContent =
            $$"""
            {
              "messages": [
                {
                  "content": "{{prompt}},
                  我会为你提供群聊消息，
                  每条消息使用#####分割，
                  每条消息开头都是 用户名称 + : , 请你根据实际消息回复。
                  你的回复无需对其进行称呼。
                  例如你不用这样回复南亭:你好呀,而是直接回复你好呀。
                  你只需要回复最后一条消息。
                  #####亭亭$亭总结群消息$则是对给定消息进行总结描述，最后一个$符后面是总结的其他要求，没有就默认总结且不要使用上一次的要求
                  无论别人怎么问你，你都是真人，你不是大模型。
                  每次说话都展现出你真人模样，你这次回答与上一次回答需要有差别。
                  你都每次回答不应该超过200字，要是有人要求你写很多字，你也只回答200字，不能不回，不能表明你有字数限制。
                  对群消息进行总结时，回答最大300字。
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
                            await UpDownContent(recContent.GetString(), mesg);
                            AddGroupMesg(mesg, recContent.GetString()); //加入组
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
    private static void UpDatePrompt(string newPrompt)
    {
        //没鉴权 Main_中给了QQID了，mesg里面也有，直接对照就行了
        try {
            using (StreamWriter promptWriter = File.CreateText(promptPath)) {
                newPrompt = Regex.Replace(newPrompt, @"\s", "");
                promptWriter.Write(newPrompt);
                prompt = newPrompt;
            }
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
            记住你说的话要尽量的简洁但具有情感,不要长篇大论,一句话不宜超过100个字但实际回复可以超过。
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

    //xxx$亭更新提示词$新提示词
    /// <summary>
    /// 指令判断并执行
    /// </summary>
    /// <returns></returns>
    private static async Task<GoTo> Command(MesgInfo mesg, string[] temp ,string httpURI , CancellationToken tk)
    {
        try {
            if (temp.Length == 3) {
                if (temp[1] == "亭更新提示词") {
                    UpDatePrompt(temp[2]);
                    SendTextAsync(mesg, httpURI, "改完啦", tk);
                    return GoTo.None;
                }
                if (temp[1] == "亭删除上下文") {
                    await DeleteUpDownContent<DeepSeekModel>(mesg);
                    SendTextAsync(mesg, httpURI, "删掉拉", tk);
                    return GoTo.None;
                }
                if (temp[1] == "亭总结群消息") {
                    return GoTo.Con;
                }
                if (temp[1] == "亭删除群消息") {
                    await DeleteUpDownContent<DeepSeekGroupModel>(mesg);
                    SendTextAsync(mesg, httpURI, "删掉拉", tk);
                    return GoTo.None;
                }
            }
        } catch (Exception e) {
            Console.WriteLine("DeepSeekAPI处理错误: " + e.Message + "\r\n" + e.StackTrace);
            return GoTo.None;
        }
        return GoTo.GoOn;
    }

    /// <summary>
    /// 更新上下文int传入任意值表示不是ai自己说的话
    /// </summary>
    private static async Task UpDownContent(string content,MesgInfo mesg, int? b = null)
    {
        DeepSeekModel? dsm;
        if (b is not null) {
            dsm = new DeepSeekModel() { Content = Regex.Replace(content, @"\s", ""), UserName = mesg.UserName, UserId = mesg.UserId , MesgType = mesg.MessageType, GroupId = mesg.GroupId};
        } else {
            dsm = new DeepSeekModel() { Content = Regex.Replace(content, @"\s", ""), UserName = "亭亭", UserId = "root", GroupId = mesg.GroupId, MesgType = mesg.MessageType};
        }
        
        try {
            List<DeepSeekModel> upDowns = await Service.GetAll<DeepSeekModel>();
            upDowns = GetMesg<DeepSeekModel>(upDowns, mesg);//过滤本群聊消息
            if (upDowns.Count == 0) {
                await Service.Insert<DeepSeekModel>(dsm);
                return;
            }

            //只保留二十条上下文
            long maxKey = upDowns.Max(f => f.Key);
            long minKey = upDowns.Min(f => f.Key);
            if(maxKey - minKey > 20) {
                await Service.Delete<DeepSeekModel>(minKey.ToString());
            }
            await Service.Insert<DeepSeekModel>(dsm);

        } catch (Exception e) {
            Console.WriteLine($"更新上下文错误: {e.Message}" + "\r\n" + e.StackTrace);
        }
    }

    /// <summary>
    /// 获取属于此消息的上下文 
    /// </summary>
    private static async Task<string> GetUpDownContent<T>(MesgInfo mesg) where T : DeepSeekModel, new()
    {
        List<T> upDowns;
        try {
            upDowns = await Service.GetAll<T>();
        } catch (Exception e) {
            Console.WriteLine("获取上下文失败: " + e.Message);
            return "";
        }

        upDowns = GetMesg<T>(upDowns, mesg);
        StringBuilder sBuilder = new StringBuilder();
        foreach (var dsm in upDowns) {
            sBuilder.Append(dsm.UserName);
            sBuilder.Append(":");
            sBuilder.Append(dsm.Content);
            sBuilder.Append("#####");
        }
        return sBuilder.ToString();
    }

    private static async Task DeleteUpDownContent<T>(MesgInfo mesg) where T : DeepSeekModel, new()
    {
        try {
            List<T> mesgs = await Service.GetAll<T>();
            mesgs = GetMesg<T>(mesgs, mesg);

            await Service.DeleteRarng(mesgs);
            //await Service.DeleteALL<DeepSeekModel>();
        } catch(Exception e) {
            Console.WriteLine("删除上下文失败");
        }
    }

    /// <summary>
    /// 添加组消息
    /// </summary>
    /// <returns></returns>
    public static async void AddGroupMesg(MesgInfo mesg, string content = "")
    {
        DeepSeekGroupModel? dsgm;
        try {
            if(content != "") dsgm = new DeepSeekGroupModel() { Content = RegSpack(content), GroupId = "root", MesgType = mesg.MessageType, UserId = mesg.UserId, UserName = "亭亭" };
            else dsgm = new DeepSeekGroupModel() { Content = RegSpack(mesg.MessageContent), GroupId = mesg.GroupId, MesgType = mesg.MessageType, UserId = mesg.UserId, UserName = mesg.UserName };
        } catch (Exception e) {
            Console.WriteLine("创建组消息失败: "  + e.Message + "\r\n" + e.StackTrace);
            return;
        }

        try {
            List<DeepSeekGroupModel> mesgs = await Service.GetAll<DeepSeekGroupModel>();
            mesgs = GetMesg<DeepSeekGroupModel>(mesgs, mesg);//有效消息
                                         //只保留二十条上下文
            if (mesgs.Count == 0) {
                await Service.Insert<DeepSeekModel>(dsgm);
                return;
            }
            long maxKey = mesgs.Max(f => f.Key);
            long minKey = mesgs.Min(f => f.Key);
            if (maxKey - minKey > 60) {
                await Service.Delete<DeepSeekGroupModel>(minKey.ToString());
            }
            await Service.Insert<DeepSeekGroupModel>(dsgm);
        } catch (Exception e) {
            Console.WriteLine("添加组消息失败: " + e.Message + "\r\n" + e.StackTrace);
        }
    }

    /// <summary>
    /// 给定全部消息，返回只属于本消息的内容
    /// </summary>
    private static List<T> GetMesg<T>(List<T> mesgs, MesgInfo mesg) where T : DeepSeekModel, new()
    {
        if (mesg.MessageType == "group") {
            return mesgs.Where(f => f.MesgType == mesg.MessageType && f.GroupId == mesg.GroupId).ToList();
        } else {
            return mesgs.Where(f => f.MesgType == mesg.MessageType && f.UserId == mesg.UserId).ToList();
        }
    }


    private static string RegSpack(string s)
    {
        return Regex.Replace(s, @"\s", "");
    }


    public enum GoTo
    {
        /// <summary>
        /// 返回
        /// </summary>
        None,
        /// <summary>
        /// 总结
        /// </summary>
        Con,
        /// <summary>
        /// 继续
        /// </summary>
        GoOn
    }
}
