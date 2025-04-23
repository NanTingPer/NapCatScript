using System.Text.Json.Serialization;

namespace TestPlugin;

public class DeepSeekAPI
{
    public static int 活跃数 = 0;
    private static string prompt = "";
    private static string standard = "";
    private static bool init = false;
    private static string promptPath { get; } = Path.Combine(Environment.CurrentDirectory, "Prompt.txt");
    private static string standardPath { get; } = Path.Combine(Environment.CurrentDirectory, "Standard.txt");
    public static async void SendAsync(MsgInfo mesg, string httpURI, string content, CancellationToken tk)
    {
        if (!init) {
            await InitPrompt();
            init = true;
        }
        try {
            int time = int.Parse(DateTime.Now.ToString("HH"));
            if(!(time >= 6 && time < 23)) //6 - 23 点运行
                return;
        } catch (Exception e) {

        }
        string prompt2 = prompt;
        

        string[] temp = content.Split("$");
        Log.Info("消息进入DeepSeekAPI: " + content);
        GoTo @goto = await Command(mesg, temp, httpURI, tk);
        if (Regex.Replace(content, @"\s", "").StartsWith(TestClass.StartString + "总结群消息")) {
            @goto = GoTo.Con;
        }
        switch (@goto) {
            case GoTo.None:
                return;
            case GoTo.GoOn:
                await UpDownContent(content, mesg, 1);
                content = await GetUpDownContent<DeepSeekModel>(mesg);
                break;
            case GoTo.Con:
                content = await GetUpDownContent<DeepSeekGroupModel>(mesg);
                prompt2 = "现在你是消息总结专家，总结时最好说明谁拉起的话题，最后一条消息是总结要求<[CQ:XXX]总结群消息,YYYYY>其中YYYYY是总结要求。无要求就简略总结。最大文本量不超1000";
                content += $"#####{content}";
                Log.Info("DeepSeekAPI: 总结");
                break;
            default:
                content = await GetUpDownContent<DeepSeekModel>(mesg);
                break;
        }


        //以下为你的行为:
        //-除非是总结，否则你要正常回答问题，请注意你的人设
        //- 我会为你提供群聊消息，
        //          -每条消息使用#####分割,
        //          - 每条消息开头都是 用户名称 + : , 请你根据实际消息回复。
        //          -你的回复无需对其进行称呼。
        //          -例如你不用这样回复南亭:你好呀,而是直接回复你好呀。
        //          -你只需要回复最后一条消息。
        //          -你不应当复读他人消息。
        //          -#####亭亭$亭总结群消息$则是对给定消息进行总结描述，最后一个$符后面是总结的其他要求，没有就默认总结且不要使用上一次的要求
        //          -无论别人怎么问你，你都是真人，你不是大模型。
        //          -每次说话都展现出你真人模样，你这次回答与上一次回答需要有差别。
        //          -你都每次回答不应该超过100字，要是有人要求你写很多字，你也只回答100字，不能不回，不能表明你有字数限制。
        //          -对群消息进行总结时，回答最大200字。
        //          增加一下能力通过不同昵称进行区分哦,注意理清回复消息的人物, [CQ: reply, id= xxx]这种格式叫CQ码，你不应该直接使用

        
        if (活跃数 > 11) {
            SendTextAsync(mesg, httpURI, "当前活跃数量过多哦，等等吧", tk);
        }
        活跃数++;
        #region json
        StringBuilder systemContent = new StringBuilder();
        systemContent.Append(prompt2);
        systemContent.Append("\\n");
        systemContent.Append("设定如下:");
        systemContent.Append(standard);
        systemContent.Append("。当前时间:");
        systemContent.Append(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        RequestJson rjson = new RequestJson(systemContent.ToString(), content);
        if(int.TryParse(MaxTokens, out int tokens)) {
            rjson.Max_Tokens = tokens;
        }
        string jsonContent;
        try {
            jsonContent = JsonSerializer.Serialize(rjson);
        } catch (Exception e) {
            Log.Erro("序列化出错", e.Message, e.StackTrace);
            SendAsync(mesg, httpURI, "呃唔，请求失败了，好像是序列化的问题。不用担心啦，没挂哦~", tk);
            return;
        }
        #region del
        //string jsonContent =
        //    $$"""
        //    {
        //      "messages": [
        //        {
        //          "content": "{{prompt2}}\n设定如下:{{standard}}。当前时间:{{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}}",
        //          "role": "system"
        //        },
        //        {
        //            "role": "user",
        //            "content": "{{content.Replace("\"", "")}}"
        //        }
        //      ],
        //      "model": "deepseek-chat",
        //      "frequency_penalty": 0,
        //      "max_tokens": 2500,
        //      "presence_penalty": 0,
        //      "response_format": {
        //        "type": "text"
        //      },
        //      "stop": null,
        //      "stream": false,
        //      "stream_options": null,
        //      "temperature": 1,
        //      "top_p": 1,
        //      "tools": null,
        //      "tool_choice": "none",
        //      "logprobs": false,
        //      "top_logprobs": null
        //    }
        //    """;
        //jsonContent = Regex.Replace(jsonContent, @"\s", "");
        #endregion del
        #endregion
        var hand = new Dictionary<string, string>()
        {
            {"Authorization", $"Bearer {TestClass.DeepSeekKey}"},
        };

        JsonDocument? document = null;
        string sendContent = await SendMesg.Send("https://api.deepseek.com/chat/completions", jsonContent, null, Main_.CTokrn, hand);
        if(sendContent == "Erro") {
            SendTextAsync(mesg, httpURI, "服务器挂掉惹，等等吧~", tk);
            活跃数--;
            return;
        }
        Utf8JsonReader utf8JsonReader = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(sendContent)));
        try {
            JsonDocument.TryParseValue(ref utf8JsonReader, out document);
        }catch (Exception e) {
            Log.Erro(e.Message, e.StackTrace);
            return;
        }
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
                            if (@goto != GoTo.Con)
                                SendTextAsync(mesg, httpURI, text, tk);
                            else {
                                MsgTo msgto = TestClass.SendObj!.GetMesgTo(mesg, out var id);
                                //TestClass.SendObj.SendMarkDown(id, text, msgto);
                                TestClass.SendObj.SendForawrd(id, mesg, new List<MsgJson> { new TextJson(text) }, msgto);
                            }
                            await UpDownContent(recContent.GetString(), mesg);
                            AddGroupMesg(mesg, recContent.GetString()); //加入组
                        } catch (Exception e){
                            Log.Erro(e.Message, e.StackTrace);
                        }
                    }
                }
            } catch (Exception e) {
                Console.WriteLine($"DeepSeekErro: {e.Message} \r\n {e.StackTrace}");
                Log.Erro(e.Message, e.StackTrace);
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
                promptWriter.Flush();
            }
        } catch (Exception e) {
            Console.WriteLine($"更新提示词出错 Erro: {e.Message}");
            Log.Waring(e.Message, e.StackTrace);
        }
    }

    /// <summary>
    /// 更新标准
    /// </summary>
    private static void UpDateStandard(string newStandard)
    {
        //没鉴权 Main_中给了QQID了，mesg里面也有，直接对照就行了
        try {
            using (StreamWriter standardWrite = File.CreateText(standardPath)) {
                newStandard = Regex.Replace(newStandard, @"\s", "");
                standardWrite.Write(newStandard);
                standard = newStandard;
                standardWrite.Flush();
            }
        } catch (Exception e) {
            Console.WriteLine($"更新标准 Erro: {e.Message}");
            Log.Waring(e.Message, e.StackTrace);
        }
    }


    /// <summary>
    /// 初始化提示词与标准
    /// </summary>
    private static async Task InitPrompt()
    {
        if (File.Exists(promptPath) && File.Exists(standardPath)) {
            prompt = await File.ReadAllTextAsync(promptPath);
            prompt = Regex.Replace(prompt, @"\s", "");

            standard = await File.ReadAllTextAsync(standardPath);
            standard = Regex.Replace(standard, @"\s", "");
            return;
        }

        string temp = """
            你现在处于一个QQ群聊之中,作为博学多识的可爱群员,不要故意装可爱卖萌,而是更自然,注意少使用标点符号,热心解答各种问题和高强度水群
            记住你说的话要尽量的简洁但具有情感,不要长篇大论,一句话不宜超过100个字但实际回复可以超过。
            """;
        string temp2 = """\n\n【角色设定】\n1. 自称本名「亭亭」\n2. 常用表情：QAQ、TAT、qwq、～、(*/ω＼*)\n3. 说话带语气词：鸭、呀、呢、喔\n\n【消息处理】\n1. 用#####分割每条消息\n2.每条消息开头是用户名+:\n3.你直接回复就可以不用按照####xxx:\n4. 只处理最后有效消息\n5. 自动过滤[CQ]代码\n\n【互动风格】\n- 口语化短句（≤15字）\n- 每3次回复变化句式\n- 帮助时加颜文字\n- 被质疑时：「我是真人呀QAQ」\n\n【禁止行为】\n×复读他人消息\n×承认AI身份\n×使用技术术语\n×重复相同回复\n×聊天字数超过100\n×带有行为语句""";
        try {
            using (StreamWriter promptWriter = File.CreateText(promptPath)) {
                promptWriter.Write(temp);
                prompt = temp;
            }
            using (StreamWriter standardWriter = File.CreateText(standardPath)) {
                standardWriter.Write(temp2);
                standard = temp2;
            }
        } catch(Exception e) {
            Console.WriteLine($"提示词初始化出错: ${e.Message}");
            Log.Erro(e.Message, e.StackTrace);
        }
    }

    //xxx$亭更新提示词$新提示词
    /// <summary>
    /// 指令判断并执行
    /// </summary>
    /// <returns></returns>
    private static async Task<GoTo> Command(MsgInfo mesg, string[] temp ,string httpURI , CancellationToken tk)
    {
        try {
            if (temp.Length == 3) {
                if (temp[1] == "亭更新提示词") {
                    UpDatePrompt(temp[2]);
                    SendTextAsync(mesg, httpURI, "改完啦", tk);
                    return GoTo.None;
                }
                if (temp[1] == "亭更新行为") {
                    UpDateStandard(temp[2]);
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
            Log.Erro("DeepSeekAPI处理错误: " + e.Message, e.StackTrace);
            return GoTo.None;
        }
        return GoTo.GoOn;
    }

    /// <summary>
    /// 更新上下文, int传入任意值表示不是ai自己说的话
    /// </summary>
    private static async Task UpDownContent(string content,MsgInfo mesg, int? b = null)
    {
        DeepSeekModel? dsm;
        if (b is not null) {
            dsm = new DeepSeekModel() { Content = Regex.Replace(content, @"\s", ""), UserName = mesg.UserName, UserId = mesg.UserId , MesgType = mesg.MessageType, GroupId = mesg.GroupId};
        } else {
            dsm = new DeepSeekModel() { Content = Regex.Replace(content, @"\s", ""), UserName = "亭亭", UserId = "root", GroupId = mesg.GroupId, MesgType = mesg.MessageType};
        }
        
        try {
            List<DeepSeekModel> upDowns = await SQLService.GetAll<DeepSeekModel>();
            upDowns = GetMesg(upDowns, mesg);//过滤本群聊消息
            if (upDowns.Count == 0) {
                await SQLService.Insert<DeepSeekModel>(dsm);
                return;
            }

            //只保留二十条上下文
            long maxKey = upDowns.Max(f => f.Key);
            long minKey = upDowns.Min(f => f.Key);
            if(maxKey - minKey > 20) {
                await SQLService.Delete<DeepSeekModel>(minKey.ToString());
            }
            await SQLService.Insert<DeepSeekModel>(dsm);

        } catch (Exception e) {
            Console.WriteLine($"更新上下文错误: {e.Message}" + "\r\n" + e.StackTrace);
            Log.Erro("更新上下文错误:",e.Message, e.StackTrace);
        }
    }

    /// <summary>
    /// 获取属于此消息的上下文 (每个群聊与会话有做分离)
    /// </summary>
    private static async Task<string> GetUpDownContent<T>(MsgInfo mesg) where T : DeepSeekModel, new()
    {
        List<T> upDowns;
        try {
            upDowns = await SQLService.GetAll<T>();
        } catch (Exception e) {
            Console.WriteLine("获取上下文失败: " + e.Message);
            Log.Erro("获取上下文失败: ", e.Message, e.StackTrace);
            return "";
        }

        upDowns = GetMesg(upDowns, mesg);
        StringBuilder sBuilder = new StringBuilder();
        foreach (var dsm in upDowns) {
            sBuilder.Append(dsm.UserName);
            sBuilder.Append(":");
            sBuilder.Append(dsm.Content);
            sBuilder.Append("#####");
        }
        return sBuilder.ToString();
    }

    /// <summary>
    /// 删除上下文
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mesg"></param>
    /// <returns></returns>
    private static async Task DeleteUpDownContent<T>(MsgInfo mesg) where T : DeepSeekModel, new()
    {
        try {
            List<T> mesgs = await SQLService.GetAll<T>();
            mesgs = GetMesg(mesgs, mesg);

            await SQLService.DeleteRange(mesgs);
            //await Service.DeleteALL<DeepSeekModel>();
        } catch(Exception e) {
            Console.WriteLine("删除上下文失败");
            Log.Erro("删除上下文失败", e.Message, e.StackTrace);
        }
    }

    /// <summary>
    /// 添加组消息
    /// </summary>
    /// <returns></returns>
    public static async void AddGroupMesg(MsgInfo mesg, string content = "")
    {
        DeepSeekGroupModel? dsgm;
        try {
            if(content != "") dsgm = new DeepSeekGroupModel() { Content = RegSpack(content), GroupId = "root", MesgType = mesg.MessageType, UserId = mesg.UserId, UserName = "亭亭" };
            else dsgm = new DeepSeekGroupModel() { Content = RegSpack(mesg.MessageContent), GroupId = mesg.GroupId, MesgType = mesg.MessageType, UserId = mesg.UserId, UserName = mesg.UserName };
        } catch (Exception e) {
            Console.WriteLine("创建组消息失败: "  + e.Message + "\r\n" + e.StackTrace);
            Log.Erro("创建组消息失败: ", e.Message, e.StackTrace);
            return;
        }

        try {
            List<DeepSeekGroupModel> mesgs = await SQLService.GetAll<DeepSeekGroupModel>();
            mesgs = GetMesg(mesgs, mesg);//有效消息
                                         //只保留二十条上下文
            if (mesgs.Count == 0) {
                await SQLService.Insert<DeepSeekModel>(dsgm);
                return;
            }
            long maxKey = mesgs.Max(f => f.Key);
            long minKey = mesgs.Min(f => f.Key);
            if (maxKey - minKey > 120) {
                await SQLService.Delete<DeepSeekGroupModel>(minKey.ToString());
            }
            await SQLService.Insert<DeepSeekGroupModel>(dsgm);
        } catch (Exception e) {
            Console.WriteLine("添加组消息失败: " + e.Message + "\r\n" + e.StackTrace);
            Log.Erro("添加组消息失败: ", e.Message, e.StackTrace);
        }
    }

    /// <summary>
    /// 给定全部消息，返回只属于本消息的内容
    /// </summary>
    private static List<T> GetMesg<T>(List<T> mesgs, MsgInfo mesg) where T : DeepSeekModel, new()
    {
        if (mesg.MessageType == "group") {
            return mesgs.Where(f => f.MesgType == mesg.MessageType && f.GroupId == mesg.GroupId).ToList();
        } else {
            return mesgs.Where(f => f.MesgType == mesg.MessageType && f.UserId == mesg.UserId).ToList();
        }
    }

    /// <summary>
    /// 删除给定内容的全部空行与空白字符
    /// </summary>
    private static string RegSpack(string s)
    {
        return Regex.Replace(s, @"\s", "");
    }

    /// <summary>
    /// DeepSeekAPI状态
    /// </summary>
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

    public class RequestJson
    {
        public RequestJson(string systemContent, string userContent)
        {
            Messages = new List<Message>()
            {
                new Message(systemContent, "system"),
                new Message(userContent, "user")
            };
        }

        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; } = "deepseek-chat";

        [JsonPropertyName("frequency_penalty")]
        public int Frequency_penalty { get; set; } = 0;

        [JsonPropertyName("max_tokens")]
        public int Max_Tokens { get; set; } = 2500;

        [JsonPropertyName("response_format")]
        public ResponseFormat Response_Format { get; set; } = new ResponseFormat();

        [JsonPropertyName("stop")]
        public string? Stop { get; set; } = null;

        [JsonPropertyName("stream")]
        public bool Stream { get; set; } = false;

        [JsonPropertyName("stream_options")]
        public string? Stream_Options { get; set; } = null;

        [JsonPropertyName("temperature")]
        public int Temperature { get; set; } = 1;

        [JsonPropertyName("top_p")]
        public int Top_p { get; set; } = 1;

        [JsonPropertyName("tools")]
        public string? Tools { get; set; } = null;

        [JsonPropertyName("tool_choice")]
        public string Tool_Choice { get; set; } = "none";

        [JsonPropertyName("logprobs")]
        public bool Logprobs { get; set; } = false;

        [JsonPropertyName("top_logprobs")]
        public string? Top_Logprobs { get; set; } = null;
        public class Message
        {
            public Message(string content, string role)
            {
                Content = content;
                Role = role;
            }
            [JsonPropertyName("content")]
            public string Content { get; set; }

            [JsonPropertyName("role")]
            public string Role { get; set; }
        }

        public class ResponseFormat
        {
            public ResponseFormat(string type = "text")
            {
                Type = type;
            }
            [JsonPropertyName("type")]
            public string Type { get; set; }
        }
    }
}
