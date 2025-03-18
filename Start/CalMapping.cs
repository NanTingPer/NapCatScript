using NapCatScript.JsonFromat;
using NapCatScript.JsonFromat.Mesgs;
using NapCatScript.MesgHandle;
using NapCatScript.MesgHandle.Parses;
using HUtils = NapCatScript.MesgHandle.Utils;
using static NapCatScript.Tool.SQLiteService;
using NapCatScript.Tool;
using System.Threading.Tasks;

namespace NapCatScript.Start
{
    public static class CalMapping
    {
        public static async Task<string> GetMap(string content)
        {
            MapModel? t = null;
            try {
                t = await Service.Get<MapModel>(content);
            } catch (Exception e){
                Console.WriteLine("发生错误: \r\n" + e.Message);
            }
            if (t is null) return content;
            return t.oldString;
        }

        public static async Task Add(MesgInfo mesg, string httpURI, string content, CancellationToken ct)
        {
            try {
                string[] mapString = content.Split("映射#")[1].Split("=>");

                if (mapString.Length < 2) {
                    await Send(mesg, httpURI, "长度不对啊", ct);
                    return;
                }

                string name = mapString[0];
                bool pd1 = ContentList.ItemName.FirstOrDefault(f => f.Equals(name)) != null;
                bool pd2 = ContentList.NPCName.FirstOrDefault(f => f.Equals(name)) != null;
                if (!pd1 && !pd2) {
                    await Send(mesg, httpURI, "你确定有这个玩意？", ct);
                }
                try {
                    await Service.CreateTable<MapModel>();
                } catch {
                    Console.WriteLine("创表失败");
                    return;
                }
                await Service.Insert<MapModel>(new MapModel() { Key = mapString[1], oldString = mapString[0] });
                await Send(mesg, httpURI, "ok啦，试试？", ct);

            } catch(Exception e) {
                Console.WriteLine("错误！\r\n" + e.Message + "\r\n" + e.StackTrace);
            }
        }

        private static async Task Send(MesgInfo mesg, string httpURI, string content ,CancellationToken ct)
        {
            string sendUri = HUtils.GetMsgURL(httpURI, mesg, out var mesgType);
            TextMesg r = new TextMesg(mesg.UserId.ToString(), mesgType, content);
            string conet = r.MesgString;
            await SendMesg.SendAsync(sendUri, conet, null, ct);
        }
    }
}
