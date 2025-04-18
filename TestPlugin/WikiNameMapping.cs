﻿using System.Threading.Tasks;
using static TestPlugin.ContentList;
namespace TestPlugin;

public static class WikiNameMapping
{
    /// <summary>
    /// .映射#头彩七=>头彩7
    /// </summary>
    public const string MapSplit = "映射#";
    public const string MapSplit2 = "=>";
    public const string DelSplit = "删除映射#";
    /// <summary>
    /// 获取映射
    /// </summary>
    /// <param name="content"> 内容 </param>
    /// <returns></returns>
    public static async Task<string> GetMap(string content)
    {
        MapModel? t = null;
        try {
            t = await Service.Get<MapModel>(content);
        } catch (Exception e){
            Console.WriteLine("发生错误: \r\n" + e.Message + "\r\n" + e.StackTrace);
        }
        if (t is null) return content;
        return t.oldString;
    }

    /// <summary>
    /// 添加映射
    /// </summary>
    /// <param name="mesg"> 消息引用 </param>
    /// <param name="httpURI"> 请求URI(例 http://127.0.0.1:6666) 不含API </param>
    /// <param name="content"> 消息内容 </param>
    /// <returns></returns>
    public static async void AddAsync(MesgInfo mesg, string httpURI, string content, CancellationToken ct)
    {
        try {
            string[] mapString = content.Split(MapSplit)[1].Split(MapSplit2);

            if (mapString.Length < 2) {
                SendTextAsync(mesg, httpURI, "长度不对啊", ct);
                return;
            }

            string name = mapString[0];
            bool pd1 = ItemName.FirstOrDefault(f => f.Equals(name)) != null;
            bool pd2 = NPCName.FirstOrDefault(f => f.Equals(name)) != null;
            bool vitem = VItems.FirstOrDefault(f => f.Equals(name)) != null;
            bool vnpc = VNPCs.FirstOrDefault(f => f.Equals(name)) != null;
            if (!pd1 && !pd2 && !vitem && !vnpc) {
                SendTextAsync(mesg, httpURI, "你确定有这个玩意？", ct);
                return;
            }
            try {
                await Service.CreateTable<MapModel>();
            } catch {
                Console.WriteLine("创表失败");
                return;
            }
            await Service.Insert(new MapModel() { Key = mapString[1], oldString = mapString[0] , UserId = mesg.UserId, CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")});
            SendTextAsync(mesg, httpURI, "ok啦，试试？", ct);

        } catch(Exception e) {
            Console.WriteLine("错误！\r\n" + e.Message + "\r\n" + e.StackTrace);
            SendTextAsync(mesg, httpURI, "错误！\r\n" + e.Message + "\r\n" + e.StackTrace, ct);
            Log.Erro(e.Message, e.StackTrace);
        }
    }

    /// <summary>
    /// 删除映射
    /// </summary>
    /// <param name="mesg"> 消息引用 </param>
    /// <param name="httpURI"> 基础URI http://127.0.0.1:6666 </param>
    /// <param name="content"> 要被删的段 </param>
    public static async void DeleteAsync(MesgInfo mesg, string httpURI, string content, CancellationToken ct)
    {
        string con = content.Split(DelSplit)[1];
        await Service.Delete<MapModel>(con);
        SendTextAsync(mesg, httpURI, "删掉啦，当然可能根本没有哦" ,ct);
    }

    /// <summary>
    /// 获取全部Mapping
    /// </summary>
    public static async Task<string> GetMappings()
    {
        List<MapModel> mappings = await Service.GetAll<MapModel>();
        var mapGroup = new Dictionary<string, List<string>>();
        foreach (var map in mappings) {
            if(mapGroup.TryGetValue(map.oldString, out var list)) { //存在
                list.Add(map.Key);
            } else {
                mapGroup.Add(map.oldString, [map.Key]);
            }
        }

        StringBuilder content = new StringBuilder();
        foreach (var kv in mapGroup) {
            content.Append(kv.Key + ":\n");
            foreach (var map in kv.Value)
                content.Append("    " + map + "\n");
        }

        return content.ToString();
    }
}
