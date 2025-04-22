using TestPlugin.Models;
namespace TestPlugin;

public static class WikiNameMapping<T> where T : MapModel, new()
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
        T? t = null;
        try {
            t = await SQLService.Get<T>(content);
        } catch (Exception e){
            Console.WriteLine("发生错误: \r\n" + e.Message + "\r\n" + e.StackTrace);
        }
        if (t is null) return content;
        return t.oldString;
    }

    //TODO 此方法业务过载
    /// <summary>
    /// 添加映射
    /// </summary>
    /// <param name="mesg"> 消息引用 </param>
    /// <param name="httpURI"> 请求URI(例 http://127.0.0.1:6666) 不含API </param>
    /// <param name="content"> 消息内容(映射#xxx=>xxx) </param>
    /// <param name="contentList"> 内容集，如果给定集合中包含消息内容的物品才进行映射 </param>
    /// <returns></returns>
    public static async void AddAsync(MesgInfo mesg, string httpURI, string content, CancellationToken ct, params IEnumerable<string>[] contentList)
    {
        try {
            string[] mapString = content.Split(MapSplit)[1].Split(MapSplit2);

            if (mapString.Length < 2) {
                SendTextAsync(mesg, httpURI, "长度不对啊", ct);
                return;
            }

            string name = mapString[0];
            bool @bool = false; //为true就行可以映射
            foreach (var worklist in contentList) {
                @bool = worklist.FirstOrDefault(f => f.Equals(name)) != null || @bool;
            }

            if (!@bool) {
                SendTextAsync(mesg, httpURI, "好像没有这个东西哦", ct);
                return;
            }
            try {
                await SQLService.CreateTable<T>();
            } catch {
                Console.WriteLine("创表失败");
                return;
            }
            await SQLService.Insert(new T() { Key = mapString[1], oldString = mapString[0] , UserId = mesg.UserId, CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UserName = mesg.UserName});
            SendTextAsync(mesg, httpURI, "ok啦，试试？", ct);

        } catch(Exception e) {
            SendTextAsync(mesg, httpURI, "错误！\r\n" + e.Message + "\r\n" + e.StackTrace, ct);
            Log.Erro(e.Message, e.StackTrace);
        }
    }

    //TODO 此方法业务过载
    /// <summary>
    /// 删除映射
    /// </summary>
    /// <param name="mesg"> 消息引用 </param>
    /// <param name="httpURI"> 基础URI http://127.0.0.1:6666 </param>
    /// <param name="content"> 要被删的段 </param>
    public static async void DeleteAsync(MesgInfo mesg, string httpURI, string content, CancellationToken ct)
    {
        string con = content.Split(DelSplit)[1];
        await SQLService.Delete<T>(con);
        SendTextAsync(mesg, httpURI, "删掉啦，当然可能根本没有哦" ,ct);
    }

    /// <summary>
    /// 获取全部Mapping
    /// </summary>
    public static async Task<string> GetMappings()
    {
        List<T> mappings = await SQLService.GetAll<T>();
        var mapGroup = new Dictionary<string, List<string>>();
        foreach (var map in mappings) {
            if(mapGroup.TryGetValue(map.oldString, out var list)) { //存在
                list.Add(map.Key + "  |  " + map.CreateTime + "  |  " + map.UserName);
            } else {
                mapGroup.Add(map.oldString, [map.Key + "  | " + map.CreateTime + "  |  " + map.UserName]);
            }
        }

        StringBuilder content = new StringBuilder();
        foreach (var kv in mapGroup) {
            content.Append(kv.Key + ":\n");
            foreach (var map in kv.Value)
                content.Append("    - " + map + "\n");
        }

        return content.ToString();
    }
}
