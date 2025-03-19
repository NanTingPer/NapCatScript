﻿using NapCatScript.MesgHandle;
using NapCatScript.Tool;
using System.Threading.Tasks;
using static NapCatScript.MesgHandle.Utils;
using static NapCatScript.Tool.SQLiteService;
namespace NapCatScript.Start
{
    public class FAQ
    {
        public static FAQ FAQI { get; set; } = new FAQ();
        public const string SplitChars = "FAQ#";
        public const string SplitChars2 = "###";

        /// <summary>
        /// 添加FAQ
        /// </summary>
        /// <param name="mesg"> 消息引用 </param>
        /// <param name="httpURI"> 请求URI(例 http://127.0.0.1:6666) 不含API </param>
        /// <param name="content"> 消息内容 </param>
        /// <returns></returns>
        public async void AddAsync(MesgInfo mesg, string httpURI, string content, CancellationToken ct)
        {
            //如果要设置权限的话，可以在这里进行
            string[] cons = content.Split(SplitChars)[1].Split(SplitChars2);
            if(cons.Length < 2) {
                SendTextAsync(mesg, httpURI, "是不是少了点什么?", ct);
                return;
            }
            var faq = new FAQModel() { Key = cons[0], Value = cons[1], CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"), UserId = mesg.UserId.ToString(), UserName = mesg.UserName };
            try {
                await Service.Insert<FAQModel>(faq);
            } catch (Exception e) {
                Console.WriteLine($"FAQ插入错误: {e.Message}\r\n {e.StackTrace}");
                return;
            }
            SendTextAsync(mesg, httpURI, "成功啦，试试？", ct);
        }

        /// <summary>
        /// 获取
        /// </summary>
        public async Task<FAQModel?> Get(string content)
        {
            FAQModel? co;
            try {
                co = await Service.Get<FAQModel>(content);
            } catch { return null; }

            return co;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async void DeleteAsync(string content)
        {
            await Service.Delete<FAQModel>(content.Split(SplitChars)[1]);
        }
    }
}
