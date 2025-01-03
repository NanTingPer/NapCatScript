using NapCatSprcit.MessagesService.SendMessagesClass;
using NapCatSprcit.MessagesService.SendMessagesClass.AllMsgJsonString;
using NapCatSprcit.SQLiteService;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using System;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using ImageRoot = NapCatSprcit.MessagesService.SendMessagesClass.AllMsgJsonString;
using PriveRoot = NapCatSprcit.MessagesService.SendMessagesClass.AllMsgJsonString.PriviteAndGroupMsg;

namespace NapCatSprcit.MessagesService.MessagesProcess
{
    public class ProcessClass
    {
        /// <summary>
        /// 消息处理核心群
        /// </summary>
        /// <param name="strings">消息列表</param>
        public static async Task<MessagesInfo?> MessagesProcess(List<string> strings)
        {
            if (strings.Count <= 0)
                return null;

            string NowData = strings.First();
            strings.RemoveAt(0);


            MessagesInfo? messages = await Task.Run(() =>
            {
                  Utf8JsonReader onlyReader = new Utf8JsonReader(new System.Buffers.ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(NowData)));
                  JsonDocument? json;
                  //解析
                  JsonDocument.TryParseValue(ref onlyReader, out json);
                  if (json == null)
                      return null;

                  //过滤
                  JsonElement rootEl = json.RootElement;
                  JsonElement? @true = EventFilter(rootEl);
                  if (@true == null)
                      return null;


                  Console.WriteLine("NapCatSprcit.MessagesService.MessagesProcess.ProcessClass : 消息抵达终点");
                  //到这里数据就yes了
                  MessagesInfo? message = GetMessageInfo(@true.Value);

                  if (message == null)
                      return null;

                  return message;

            });

            if(messages == null) return null;
            //处理 发送消息
            await SendMessageAsync(messages);

            return messages;
        }
        //user_id 用户id          root
        //raw_message 原始消息    root

        private static JsonElement? EventFilter(JsonElement root)
        {
            if(root.TryGetProperty("post_type",out JsonElement value))
            {
                if (value.GetString() == "message")
                    return root;
                else 
                    return null;
            }
            return null;
        }

        /// <summary>
        /// 给JsonRoot 返回
        /// </summary>
        /// <param name="root"> Json的主对象 </param>
        /// <returns></returns>
        private static MessagesInfo? GetMessageInfo(JsonElement root)
        {
            JsonElement user_id;
            JsonElement message;
            JsonElement message_type;

            bool user_id_bool = root.TryGetProperty("user_id", out user_id);
            bool message_bool = root.TryGetProperty("raw_message", out message);
            bool message_type_bool = root.TryGetProperty("message_type", out message_type);

            if (!user_id_bool || !message_bool || !message_type_bool)
                return null;

            return new MessagesInfo() { MessageContent = message.GetString(),MessageType = message_type.GetString(),UserId = user_id.GetUInt64() };

        }


        /// <summary>
        /// 请传入 MessagesProcess MessagesInfo
        /// </summary>
        public static async Task SendMessageAsync(MessagesInfo message)
        {
            var msgSend = PublicProperty.Messages;
            var content = message.MessageContent;
            Console.WriteLine("NapCatSprcit.MessagesService.MessagesProcess.ProcessClass : 开始检查并发送消息");
            if((content.Contains("添加:") || content.Contains("添加：")) && 
               (content.Contains("回答:") || content.Contains("回答：")))
            {
                await SQLAddData(message);
            }
            else
            {
                DataModel r = await Service.GetAsync(content);
                if(r != null)
                {
                    Console.WriteLine("正在获取数据");
                    string messageSendText = msgSend.buildMessagesText(message, r.Value); //构建消息体
                    string URL = GetMsgURL(message);    //获取发送消息的API

                    Console.WriteLine("目标URL: " + URL);

                    Console.WriteLine("目标回复: " + await msgSend.SendPostMessagesAsync(URL, messageSendText, Encoding.UTF8));
                }
            }

        }

        #region 浏览器截图 不可用
        #region 网页截图 不可用
        /*
        if (message.MessageContent.Contains("网页截图") && (message.MessageType == "private" || message.MessageType == "group"))
        {
            Console.WriteLine("消息进入了网页截图");
            Console.WriteLine(message);
            await PrivateImage(message);
        }
        else if(message.MessageContent.Contains("原版查询") && (message.MessageType == "private" || message.MessageType == "group"))
        {
            Console.WriteLine("消息进入了原版查询");
            await TerrariaWikiImage(message);
        }
        */
        #endregion 网页截图 不可用

        #region 私聊 或者 群聊链接 发送图片 不可用
        /*
        public static async Task PrivateImage(MessagesInfo message)
        {
            Messages? mes = PublicProperty.Messages;
            string URI = PublicProperty.HttpURI;
            var weburl = message.MessageContent.Split(" ")[1].Trim();

            string fileTempPath = Path.GetTempFileName();

            await Browser(weburl, fileTempPath);

            URI = GetImageMessagesURL(message);
            ImageRoot.ImageMessage.Root e = new ImageMessage(message.UserId.ToString(), MsgTypeEnum.image, fileTempPath).MassageJsonObject;
            string messages = JsonSerializer.Serialize(e, new JsonSerializerOptions() { WriteIndented = false });

            await mes.SendPostMessagesAsync(URI, messages, Encoding.UTF8);
            System.IO.File.Delete(fileTempPath);
        }
        */
        #endregion

        #region 原版Wiki
        /*
        public static async Task TerrariaWikiImage(MessagesInfo message)
        {
            Messages? mes = PublicProperty.Messages;
            string URI = PublicProperty.HttpURI;
            var weburl = message.MessageContent.Split(" ")[1].Trim();
            Console.WriteLine("已经获取查询物品: " + weburl);

            string fileTempPath = Path.GetTempFileName();
            string url = "https://terraria.wiki.gg/zh/wiki/" + weburl;
            await Browser(url, fileTempPath);
            Console.WriteLine("保存完毕");

            //获取请求API
            URI = GetImageMessagesURL(message);

            ImageRoot.ImageMessage.Root e = new ImageMessage(message.UserId.ToString(), MsgTypeEnum.image, fileTempPath).MassageJsonObject;
            string messages = JsonSerializer.Serialize(e, new JsonSerializerOptions() { WriteIndented = false });
            await mes.SendPostMessagesAsync(URI, messages, Encoding.UTF8);
            System.IO.File.Delete(fileTempPath);
        }
        */
        #endregion

        /// <summary>
        /// 浏览器截图
        /// </summary>
        /// <param name="url"> 目标URL </param>
        /// <param name="fileTempPath"> 保存到哪里 </param>
        /// <returns></returns>
        /*public static async Task Browser(string url,string fileTempPath)
        {
            var bf = new BrowserFetcher();
            //下载浏览器
            await bf.DownloadAsync();
            Console.WriteLine("正在下载浏览器");
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            Console.WriteLine("下载完成");
            var page = await browser.NewPageAsync();
            Console.WriteLine("打开了浏览器页面");

            await page.SetViewportAsync(new ViewPortOptions() { Height = 1920, Width = 1080 });
            Console.WriteLine("改变了大小");

            await page.GoToAsync(url);
            Console.WriteLine("访问完毕");

            await page.ScreenshotAsync(fileTempPath);
            Console.WriteLine("保存了" + fileTempPath);

            await page.CloseAsync();
            await browser.CloseAsync();
        }*/
        #endregion


        ///<summary>
        ///获取Msg消息URL 基本消息
        ///</summary>
        public static string GetMsgURL(MessagesInfo message)
        {
            if (message.MessageType == "group")
            {
                if (PublicProperty.HttpURI.EndsWith("/"))
                {
                    return PublicProperty.HttpURI + AllWebAPI.GroupMsgNoX;
                }
                else
                {
                    return PublicProperty.HttpURI + AllWebAPI.GroupMsg;
                }
            }

            if (message.MessageType == "private")
            {

                if (PublicProperty.HttpURI.EndsWith("/"))
                {
                    return PublicProperty.HttpURI + AllWebAPI.PrivateMsgNoX;
                }
                else
                {
                    return PublicProperty.HttpURI + AllWebAPI.PrivateMsg;
                }
            }

            return "";
        }

        /// <summary>
        /// 往数据库添加自定义回答信息
        /// </summary>
        /// <param name="message"> 消息信息 判断内容 </param>
        /// <returns></returns>
        public static async Task SQLAddData(MessagesInfo message)
        {
            var content = message.MessageContent;
            var str = "";
            var key = "";
            var value = "";
            string[] constr = new string[] { "回答:", "回答：" };

            str = content.Replace("添加:", "").Replace("添加：", "");

            foreach (var c in constr)
            {
                if (str.Contains(c))
                {
                    var sssss = str.Split(c);
                    key = sssss[0].Trim();
                    value = sssss[1].Trim();
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine($"向数据库添加数据：{key}=> {value}");
                await Service.SetAsync(new DataModel() { Key = key, Value = value });
            }
        }
    }

    /// <summary>
    /// 数据信息
    /// </summary>
    public class MessagesInfo()
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public ulong UserId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set; } = string.Empty;

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType { get; set; } = string.Empty;

        public override string ToString()
        {

            return UserId + " " + MessageContent + " " + MessageType;
        }
    }

}
