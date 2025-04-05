using NapCatScript.JsonFromat;
using NapCatScript.JsonFromat.Mesgs;
using NapCatScript.MesgHandle.Parses;
using NapCatScript.JsonFromat.JsonModel;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
            if (uri.EndsWith('/')) return uri + API.GroupMsgNoX;
            else return uri + API.GroupMsg;
        }

        if (message.MessageType == "private") {
            mesg = MesgTo.user;
            if (uri.EndsWith('/')) return uri + API.PrivateMsgNoX;
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
    public string GetFriendListAPI { get => HttpURI + nameof(get_friend_list); }
    public string GetFriendsWithCategoryAPI { get => HttpURI + nameof(get_friends_with_category); }
    public string GetProFileLikeAPI { get => HttpURI + nameof(get_profile_like); }
    public string GetStrangerInfoAPI { get => HttpURI + nameof(get_stranger_info); }
    public string SendLikeAPI { get => HttpURI + nameof(send_like); }
    public string SetFriendAddRequestAPI { get => HttpURI + nameof(set_friend_add_request); }
    public string SetOnlineStatusAPI { get => HttpURI + nameof(set_online_status); }
    public string SetQQAvatarAPI { get => HttpURI + nameof(set_qq_avatar); }
    public string SetSelfLongnickAPI { get => HttpURI + nameof(set_self_longnick); }
    public string UploadPrivateFileAPI { get => HttpURI + nameof(upload_private_file); }
    public string SendPrivateMsg { get => HttpURI + "send_private_msg"; }
    public string SendGroupMsg { get => HttpURI + "send_group_msg"; }
    public string HttpURI { get; set; } = "";
    public Send(string httpURI)
    {
        if (httpURI.EndsWith('/'))
            HttpURI = httpURI;
        else HttpURI = httpURI + '/';
    }

    ///<summary>
    ///获取Msg消息URL 基本消息(API访问链接)
    ///<para> uri是原始链接 例如: http://127.0.0.1:6666 </para>
    ///</summary>
    private string GetMsgSendToURI(MesgTo mesg)
    {
        switch (mesg) {
            case MesgTo.group:
                return HttpURI + "send_group_msg";
            case MesgTo.user:
                return HttpURI + "send_private_msg";
        }
        return "";
    }

    public MesgTo GetMesgTo(MesgInfo mesginfo, out string id)
    {
        if (mesginfo.MessageType == "group") {
            id = mesginfo.GroupId;
            return MesgTo.group;
        } else {
            id = mesginfo.UserId;
            return MesgTo.user;
        }
    }
    #region 上传私聊文件 upload_private_file
    /// <summary>
    /// 上传私聊文件
    /// </summary>
    /// <param name="user_id"> 用户id </param>
    /// <param name="file"> 文件路径 </param>
    /// <param name="name"> 目标名称 </param>
    public async Task<bool> UploadPrivateFileAsync(string user_id, string file, string name)
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(UploadPrivateFileAPI, new upload_private_file(user_id, file, name).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            Loging.Log.Erro("上传私聊文件", e.Message, e.StackTrace);
            return false;
        }
    }
    /// <summary>
    /// 上传私聊文件
    /// </summary>
    /// <param name="user_id"> 用户id </param>
    /// <param name="file"> 文件路径 </param>
    /// <param name="name"> 目标名称 </param>
    public async void UploadPrivateFile(string user_id, string file, string name)
    {
        try {
            await SendMesg.Send(UploadPrivateFileAPI, new upload_private_file(user_id, file, name).JsonText);
        } catch (Exception e) {
            Loging.Log.Erro("上传私聊文件", e.Message, e.StackTrace);
        }
    }
    #endregion

    #region 设置个性签名 set_self_longnick
    /// <summary>
    /// 设置个性签名
    /// </summary>
    public async Task<bool> SetSelfLongnickAsync(string content)
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(SetSelfLongnickAPI, new set_self_longnick(content).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            Loging.Log.Erro("设置个性签名", e.Message, e.StackTrace);
            return false;
        }
    }
    /// <summary>
    /// 设置个性签名
    /// </summary>
    public async void SetSelfLongnick(string content)
    {
        try {
            await SendMesg.Send(SetSelfLongnickAPI, new set_self_longnick(content).JsonText);
        } catch (Exception e) {
            Loging.Log.Erro("设置个性签名", e.Message, e.StackTrace);
        }
    }
    #endregion

    #region 设置QQ头像 set_qq_avatar
    /// <summary>
    /// 设置QQ头像
    /// </summary>
    public async Task<bool> SetQQAvatarAsync(string path)
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(SetQQAvatarAPI, new set_qq_avatar(path).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            Loging.Log.Erro("设置QQ头像", e.Message, e.StackTrace);
            return false;
        }
    }
    /// <summary>
    /// 设置QQ头像
    /// </summary>
    public async void SetQQAvatar(string path)
    {
        try {
            await SendMesg.Send(SetQQAvatarAPI, new set_qq_avatar(path).JsonText);
        } catch (Exception e) {
            Loging.Log.Erro("设置QQ头像", e.Message, e.StackTrace);
        }
    }
    #endregion

    #region 设置在线状态 set_online_status
    /// <summary>
    /// 设置在线状态
    /// </summary>
    public async Task<bool> SetOnlineStatusAsync(set_online_status.OnlineType type)
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(SetOnlineStatusAPI, new set_online_status(type).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            Loging.Log.Erro("设置在线状态", e.Message, e.StackTrace);
            return false;
        }
    }
    /// <summary>
    /// 设置在线状态
    /// </summary>
    public async void SetOnlineStatus(set_online_status.OnlineType type)
    {
        try {
            await SendMesg.Send(SetOnlineStatusAPI, new set_online_status(type).JsonText);
        } catch (Exception e) {
            Loging.Log.Erro("设置在线状态", e.Message, e.StackTrace);
        }
    }
    #endregion

    #region 处理好友请求 set_friend_add_request
    /// <summary>
    /// 处理好友请求
    /// </summary>
    /// <param name="flag"> 目标id </param>
    /// <param name="approve"> 是否同意 </param>
    /// <param name="remark"> 设置备注 </param>
    public async Task<bool> SetFriendAddRequestAsync(string flag, bool approve,string remark)
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(SetFriendAddRequestAPI, new set_friend_add_request(flag, approve, remark).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            Loging.Log.Erro("处理好友请求", e.Message, e.StackTrace);
            return false;
        }
    }
    /// <summary>
    /// 处理好友请求
    /// </summary>
    /// <param name="flag"> 目标id </param>
    /// <param name="approve"> 是否同意 </param>
    /// <param name="remark"> 设置备注 </param>
    public async void SetFriendAddRequest(string flag, bool approve, string remark)
    {
        try {
            await SendMesg.Send(SetFriendAddRequestAPI, new set_friend_add_request(flag, approve, remark).JsonText);
        } catch (Exception e) {
            Loging.Log.Erro("处理好友请求", e.Message, e.StackTrace);
        }
    }
    #endregion

    #region 点赞 send_like
    /// <summary>
    /// 给目标用户点赞
    /// </summary>
    public async Task<bool> SendLikeAsync(string user_id, int num)
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(SendLikeAPI, new send_like(user_id, num).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            Loging.Log.Erro(e.Message, e.StackTrace);
            return false;
        }
    }
    /// <summary>
    /// 给目标用户点赞
    /// </summary>
    public async void SendLike(string user_id, int num)
    {
        try {
            await SendMesg.Send(SendLikeAPI, new send_like(user_id, num).JsonText);
        } catch (Exception e) {
            Loging.Log.Erro(e.Message, e.StackTrace);
        }
    }
    #endregion

    #region 获取帐号信息 get_stranger_info
    /// <summary>
    /// 获取点赞列表
    /// </summary>
    public async Task<get_stranger_infoReturn?> GetStrangerInfoAsync(string user_id)
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(GetStrangerInfoAPI, new get_stranger_info(user_id).JsonText);
            if ((int)httpc.StatusCode != 200)
                return null;
            return JsonSerializer.Deserialize<get_stranger_infoReturn>(await httpc.Content.ReadAsStringAsync());
        } catch (Exception e) {
            Loging.Log.Erro(e.Message, e.StackTrace);
            return null;
        }
    }
    #endregion

    #region 获取点赞列表 get_profile_like
    /// <summary>
    /// 获取点赞列表
    /// </summary>
    public async Task<get_profile_like?> GetProFileLikeAsync()
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(GetProFileLikeAPI, "{}");
            if ((int)httpc.StatusCode != 200)
                return null;
            return JsonSerializer.Deserialize<get_profile_like>(await httpc.Content.ReadAsStringAsync());
        } catch (Exception e) {
            Loging.Log.Erro(e.Message, e.StackTrace);
            return null;
        }
    }
    #endregion

    #region 获取好友信息分组列表 get_friends_with_category
    /// <summary>
    /// 获取好友信息分组列表
    /// </summary>
    public async Task<get_friends_with_category?> GetFriendsWithCategoryAsync()
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(GetFriendsWithCategoryAPI, "{}");
            if ((int)httpc.StatusCode != 200)
                return null;
            return JsonSerializer.Deserialize<get_friends_with_category>(await httpc.Content.ReadAsStringAsync());
        } catch (Exception e) {
            Loging.Log.Erro(e.Message, e.StackTrace);
            return null;
        }
    }
    #endregion

    #region 获取好友列表 get_friend_list
    /// <summary>
    /// 获取好友列表
    /// </summary>
    public async Task<get_friend_listReturn?> GetFriendListAsync()
    {
        HttpResponseMessage httpc;
        try {
            httpc = await SendMesg.Send(GetFriendListAPI, new get_friend_list().JsonText);
            if ((int)httpc.StatusCode != 200)
                return null;
            return JsonSerializer.Deserialize<get_friend_listReturn>(await httpc.Content.ReadAsStringAsync());
        } catch (Exception e) {
            Loging.Log.Erro(e.Message, e.StackTrace);
            return null;
        }
    }
    #endregion

    #region 获取群聊卡片 ArkShareGroup

        /// <summary>
        /// 获取群卡片
        /// </summary>
    public async Task<ArkShareGroupReturn?> GetArkShareGroupAsync(string group_id)
    {
        HttpResponseMessage? httpc = null;
        try {
            var requestJson = new ArkShareGroup(group_id).ToString();
            httpc = await SendMesg.Send(ArkShareGroupAPI, requestJson, null);
        } catch (Exception e){
            Loging.Log.Erro(e.Message, e.StackTrace);
            return null;
        }
        if ((int)httpc.StatusCode != 200) return null;
        return JsonSerializer.Deserialize<ArkShareGroupReturn>(await httpc.Content.ReadAsStringAsync());
    }
    #endregion

    #region 获取推荐好友或者群聊卡片 ArkSharePeer

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

    #region 发送markDown
    /// <summary>
    /// 此方法发送的MarkDown用户id为123456. 昵称为 匿名
    /// </summary>
    /// <param name="id"> 群ID / 个人ID </param>
    /// <param name="content"> 内容 </param>
    /// <param name="type"> 类型 </param>
    public async void SendMarkDown(string id, string content, MesgTo type)
    {
        var MarkDownJson = new MarkDownJson(content);
        var 二级转发消息 = new TwoForwardMsgJson(MarkDownJson);
        var 一级转发消息 = new ForwardData(二级转发消息);
        ForwardMsgJson postContent =  new ForwardMsgJson(id, 一级转发消息, type);
        string postContents = JsonSerializer.Serialize(postContent);

        string POSTURI = HttpURI + "send_forward_msg";
        HttpResponseMessage? postReturnContent = await SendMesg.Send(POSTURI, postContents);
        await postReturnContent.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 此方法发送的MarkDown 聊天记录消息用户为拉起用户
    /// </summary>
    /// <param name="id"> 群ID / 个人ID </param>
    /// <param name="content"> 内容 </param>
    /// <param name="type"> 类型 </param>
    public async void SendMarkDown(string id, string content, MesgInfo mesg, MesgTo type)
    {
        var MarkDownJson = new MarkDownJson(content);
        var 二级转发消息 = new TwoForwardMsgJson(mesg.UserId, mesg.UserName, MarkDownJson);
        var 一级转发消息 = new ForwardData(mesg.UserId, mesg.UserName, 二级转发消息);
        ForwardMsgJson postContent = new ForwardMsgJson(id, 一级转发消息, type);
        string postContents = JsonSerializer.Serialize(postContent);

        string POSTURI = HttpURI + "send_forward_msg";
        HttpResponseMessage? postReturnContent = await SendMesg.Send(POSTURI, postContents);
        await postReturnContent.Content.ReadAsStringAsync();
    }

    #endregion
}

public class SendCN
{
    private Send send;
    public SendCN(Send sd)
    {
        send = sd;
    }

    public Task<ArkShareGroupReturn?> 获取群聊卡片(string 群号) => send.GetArkShareGroupAsync(群号);
    public Task<ArkSharePeerReturn?> 获取推荐好友或者群聊卡片(string id, ArkSharePeerEnum 类型) => send.GetArkSharePeerAsync(id, 类型);
    public void 创建收藏内容(string 收藏标题, string 收藏内容) => send.CreateCollection(收藏标题, 收藏内容);
    public Task<bool> 创建收藏内容Async(string 收藏标题, string 收藏内容) => send.CreateCollectionAsync(收藏标题, 收藏内容);
    public void 删除好友(string 用户ID, bool 是否拉黑, bool 是否双向删除) => send.DeleteFriend(用户ID, 是否拉黑, 是否双向删除);
    public Task<bool> 删除好友Async(string 用户ID, bool 是否拉黑, bool 是否双向删除) => send.DeleteFriendAsync(用户ID, 是否拉黑, 是否双向删除);
    public Task<get_friend_listReturn?> 获取好友列表Async() => send.GetFriendListAsync();
    public Task<get_friends_with_category?> 获取好友信息分组列表Async() => send.GetFriendsWithCategoryAsync();
    public Task<get_profile_like?> 获取点赞列表Async() => send.GetProFileLikeAsync();
    public Task<get_stranger_infoReturn?> 获取帐号信息Async(string 用户ID) => send.GetStrangerInfoAsync(用户ID);
    public Task<bool> 点赞Async(string 用户ID, int 次数) => send.SendLikeAsync(用户ID, 次数);
    public void 点赞(string 用户ID, int 次数) => send.SendLike(用户ID, 次数);
    public Task<bool> 处理好友请求Async(string 目标ID, bool 是否同意, string 备注) => send.SetFriendAddRequestAsync(目标ID, 是否同意, 备注);
    public void 处理好友请求(string 目标ID, bool 是否同意, string 备注) => send.SetFriendAddRequest(目标ID, 是否同意, 备注);
    public Task<bool> 设置在线状态Async(set_online_status.OnlineType type) => send.SetOnlineStatusAsync(type);
    public void 设置在线状态(set_online_status.OnlineType type) => send.SetOnlineStatus(type);
    public Task<bool> 设置QQ头像Async(string 文件路径) => send.SetQQAvatarAsync(文件路径);
    public void 设置QQ头像(string 文件路径) => send.SetQQAvatar(文件路径);
    public Task<bool> 设置个性签名Async(string 内容) => send.SetSelfLongnickAsync(内容);
    public void 设置个性签名(string 内容) => send.SetSelfLongnick(内容);
    public Task<bool> 上传私聊文件Async(string 用户ID, string 文件路径, string 名称) => send.UploadPrivateFileAsync(用户ID, 文件路径, 名称);
    public void 上传私聊文件(string 用户ID, string 文件路径, string 名称) => send.UploadPrivateFile(用户ID, 文件路径, 名称);
    public void 发送MarkDown(string 目标, string 内容, MesgInfo 消息引用, MesgTo 去处) => send.SendMarkDown(目标, 内容, 消息引用, 去处);
    /// <summary>
    /// 此方法发送的MarkDown用户id为123456. 昵称为 匿名
    /// </summary>
    /// <param name="id"> 群ID / 个人ID </param>
    /// <param name="content"> 内容 </param>
    /// <param name="type"> 类型 </param>
    public void 发送MarkDown(string 目标, string 内容, MesgTo 去处) => send.SendMarkDown(目标, 内容, 去处);

}
