using NapCatScript.JsonFromat;
using NapCatScript.JsonFromat.Mesgs;
using NapCatScript.MesgHandle.Parses;
using NapCatScript.JsonFromat.JsonModel;
using System.Threading.Tasks;
using System.Net.Http;

namespace NapCatScript.MesgHandle;
public static class Utils
{
    ///<summary>
    ///获取Msg消息URL 基本消息(API访问链接)
    ///<para> uri是原始链接 例如: http://127.0.0.1:6666 </para>
    ///</summary>
    public static string GetMsgURL(string uri, MesgInfo message, out MesgTo mesg)
    {
        if (message.MessageType == "group") {
            mesg = MesgTo.group;
            if (uri.EndsWith("/")) return uri + API.GroupMsgNoX;
            else return uri + API.GroupMsg;
        }

        if (message.MessageType == "private") {
            mesg = MesgTo.user;
            if (uri.EndsWith("/")) return uri + API.PrivateMsgNoX;
            else return uri + API.PrivateMsg;
        }
        mesg = MesgTo.user;
        return "";
    }

    /// <summary>
    /// 获取目标ID
    /// <para>如果是群消息，返回群ID</para>
    /// </summary>
    public static string GetUserId(MesgInfo mesg)
    {
        if (mesg.GroupId != string.Empty)
            return mesg.GroupId;
        return mesg.UserId;
    }

    /// <summary>
    /// 利用MesgInfo的内容，将文本发送出去
    /// </summary>
    /// <param name="mesg"> 消息引用 </param>
    /// <param name="httpURI"> 目标基础uri 例: http://127.0.0.1:6666 </param>
    /// <param name="content"> 消息内容 </param>
    /// <returns></returns>
    public static async void SendTextAsync(MesgInfo mesg, string httpURI, string content, CancellationToken ct)
    {
        string sendUri = GetMsgURL(httpURI, mesg, out var mesgType);
        TextMesg r = new TextMesg(GetUserId(mesg), mesgType, content);
        string conet = r.MesgString;
        await SendMesg.Send(sendUri, conet, null, ct);
    }
}

public class Send
{
    public string ArkShareGroupAPI { get => HttpURI + nameof(ArkShareGroup); }
    public string ArkSharePeerAPI { get => HttpURI + nameof(ArkSharePeer); }
    public string CreateCollectionAPI { get => HttpURI + nameof(create_collection); }
    public string DeleteFriendAPI { get => HttpURI + nameof(delete_friend); }
    public string HttpURI { get; set; } = "";
    public Send(string httpURI)
    {
        if (httpURI.EndsWith("/"))
            HttpURI = httpURI;
        else HttpURI = httpURI + "/";
    }

    #region 获取群聊卡片 ArkShareGroup
    public Task<ArkShareGroupReturn?> 获取群聊卡片(string group_id) => GetArkShareGroupAsync(group_id);

    /// <summary>
    /// 获取群卡片
    /// </summary>
    public async Task<ArkShareGroupReturn?> GetArkShareGroupAsync(string group_id)
    {
        HttpResponseMessage? httpc = null;
        try {
            httpc = await SendMesg.Send(ArkShareGroupAPI, new ArkShareGroup(group_id).ToString(), null);
        } catch (Exception e){
            Loging.Log.Erro(e.Message, e.StackTrace);
            return null;
        }
        if ((int)httpc.StatusCode != 200) return null;
        return JsonSerializer.Deserialize<ArkShareGroupReturn>(await httpc.Content.ReadAsStringAsync());
    }
    #endregion

    #region 获取推荐好友或者群聊卡片 ArkSharePeer
    public Task<ArkSharePeerReturn?> 获取推荐好友或者群聊卡片(string id, ArkSharePeerEnum type) => GetArkSharePeerAsync(id, type);

    /// <summary>
    /// 获取推荐好友/群聊卡片
    /// </summary>
    public async Task<ArkSharePeerReturn?> GetArkSharePeerAsync(string id,ArkSharePeerEnum type)
    {
        HttpResponseMessage? httpc = null;
        try {
            httpc = await SendMesg.Send(ArkSharePeerAPI, new ArkSharePeer(id, type).ToString(), null);
        } catch (Exception e) {
            Loging.Log.Erro(e.Message, e.StackTrace);
            return null;
        }
        if ((int)httpc.StatusCode != 200) return null;
        return JsonSerializer.Deserialize<ArkSharePeerReturn>(await httpc.Content.ReadAsStringAsync());
    }

    #endregion

    #region 创建收藏内容 create_collection
    public void 创建收藏内容(string 收藏标题, string 收藏内容) => CreateCollection(收藏标题, 收藏内容);
    public Task<bool> 创建收藏内容Async(string 收藏标题, string 收藏内容) => CreateCollectionAsync(收藏标题, 收藏内容);
    /// <summary>
    /// 创建收藏内容
    /// </summary>
    /// <param name="bried"> 收藏标题 </param>
    /// <param name="rowdata"> 收藏内容 </param>
    public async void CreateCollection(string bried, string rowdata)
    {
        try {
            _ = await SendMesg.Send(CreateCollectionAPI, new create_collection(bried, rowdata).JsonText);
        } catch (Exception e) {
            Loging.Log.Erro(e.Message, e.StackTrace);
        }
    }

    /// <summary>
    /// 创建收藏内容，成功返回true
    /// </summary>
    /// <param name="bried"> 收藏标题 </param>
    /// <param name="rowdata"> 收藏内容 </param>
    public async Task<bool> CreateCollectionAsync(string bried, string rowdata)
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(CreateCollectionAPI, new create_collection(bried, rowdata).JsonText);
            if ((int)httpc.StatusCode != 200)
                return false;
            return true;
        } catch (Exception e) {
            Loging.Log.Erro(e.Message, e.StackTrace);
            return false;
        }
    }

    #endregion

    #region 删除好友 delete_friend
    public void 删除好友(string 用户ID, bool 是否拉黑, bool 是否双向删除) => DeleteFriend(用户ID, 是否拉黑, 是否双向删除);
    public Task<bool> 删除好友Async(string 用户ID, bool 是否拉黑, bool 是否双向删除) => DeleteFriendAsync(用户ID, 是否拉黑, 是否双向删除);
    /// <summary>
    /// 删除好友
    /// </summary>
    /// <param name="user_id"> 用户id </param>
    /// <param name="tempBlock"> 是否拉黑 </param>
    /// <param name="tempBothDel"> 是否双向删除 </param>
    public async void DeleteFriend(string user_id, bool tempBlock, bool tempBothDel)
    {
        try {
            _ = await SendMesg.Send(DeleteFriendAPI, new delete_friend(user_id, tempBlock, tempBothDel).JsonText);
        } catch (Exception e) {
            Loging.Log.Erro(e.Message, e.StackTrace);
        }
    }

    /// <summary>
    /// 删除好友，成功返回true
    /// </summary>
    /// <param name="user_id"> 用户id </param>
    /// <param name="tempBlock"> 是否拉黑 </param>
    /// <param name="tempBothDel"> 是否双向删除 </param>
    public async Task<bool> DeleteFriendAsync(string user_id, bool tempBlock, bool tempBothDel)
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(DeleteFriendAPI, new delete_friend(user_id, tempBlock, tempBothDel).JsonText);
            if ((int)httpc.StatusCode != 200)
                return false;
            return true;
        } catch (Exception e) {
            Loging.Log.Erro(e.Message, e.StackTrace);
            return false;
        }
    }
    #endregion
}

