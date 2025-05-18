using NapCatScript.Core.JsonFormat.GroupManagers;
using System.Net;
using System.Net.Http.Headers;
using NapCatScript.Core.JsonFormat.GetJsons;
using static NapCatScript.Core.MsgHandle.SendMsg;

namespace NapCatScript.Core.MsgHandle;
public class Send
{
    public HttpClient HttpClient { get; init; }
    
    public Send(string httpURI, string token = "")
    {
        if (httpURI.EndsWith('/'))
            HttpURI = httpURI;
        else HttpURI = httpURI + '/';
        
        HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    #region API
    private string ArkShareGroupAPI { get => HttpURI + nameof(ArkShareGroup); }
    private string ArkSharePeerAPI { get => HttpURI + nameof(ArkSharePeer); }
    private string CreateCollectionAPI { get => HttpURI + nameof(create_collection); }
    private string DeleteFriendAPI { get => HttpURI + nameof(delete_friend); }
    private string GetFriendListAPI { get => HttpURI + nameof(get_friend_list); }
    private string GetFriendsWithCategoryAPI { get => HttpURI + nameof(get_friends_with_category); }
    private string GetProFileLikeAPI { get => HttpURI + nameof(get_profile_like); }
    private string GetStrangerInfoAPI { get => HttpURI + nameof(get_stranger_info); }
    private string GetGroupListAPI { get => HttpURI + nameof(get_group_list); }
    private string SendLikeAPI { get => HttpURI + nameof(send_like); }
    private string SetFriendAddRequestAPI { get => HttpURI + nameof(set_friend_add_request); }
    private string SetOnlineStatusAPI { get => HttpURI + nameof(set_online_status); }
    private string SetQQAvatarAPI { get => HttpURI + nameof(set_qq_avatar); }
    private string SetSelfLongnickAPI { get => HttpURI + nameof(set_self_longnick); }
    private string UploadPrivateFileAPI { get => HttpURI + nameof(upload_private_file); }
    private string SendPrivateMsgAPI { get => HttpURI + "send_private_msg"; }
    private string SendGroupMsgAPI { get => HttpURI + "send_group_msg"; }
    private string SendMsgAPI { get => HttpURI + "send_msg"; }
    private string GroupBanAPI { get => HttpURI + nameof(set_group_ban); }
    private string GroupKickAPI { get => HttpURI + nameof(set_group_kick); }
    private string PoKeAPI { get => HttpURI + nameof(send_poke); }
    private string HttpURI { get; set; } = "";
    #endregion API

    #region 辅助方法
    ///<summary>
    ///获取Msg消息URL 基本消息(API访问链接)
    ///<para> uri是原始链接 例如: http://127.0.0.1:6666 </para>
    ///</summary>
    private string GetMsgSendToURI(MsgTo mesg)
    {
        switch (mesg) {
            case MsgTo.group:
                return HttpURI + "send_group_msg";
            case MsgTo.user:
                return HttpURI + "send_private_msg";
        }
        return "";
    }

    public MsgTo GetMesgTo(MsgInfo mesginfo, out string id)
    {
        if (mesginfo.MessageType == "group") {
            id = mesginfo.GroupId.ToString();
            return MsgTo.group;
        } else {
            id = mesginfo.UserId.ToString();
            return MsgTo.user;
        }
    }
    #endregion

    #region 上传私聊文件 upload_private_file

    /// <summary>
    /// 上传私聊文件，通过info中的消息类型决定用户id
    /// </summary>
    /// <param name="info"> 消息信息 </param>
    /// <param name="file"> 文件路径 </param>
    /// <param name="name"> 目标名称 </param>
    public async void UploadPrivateFileAsync(MsgInfo info, string file, string name) => await UploadPrivateFileAsync(info.GetId(), file, name);

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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, UploadPrivateFileAPI, new upload_private_file(user_id, file, name).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            InstanceLog.Erro("上传私聊文件", e.Message, e.StackTrace);
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
            await MsgHandle.SendMsg.PostSend(HttpClient, UploadPrivateFileAPI, new upload_private_file(user_id, file, name).JsonText);
        } catch (Exception e) {
            InstanceLog.Erro("上传私聊文件", e.Message, e.StackTrace);
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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, SetSelfLongnickAPI, new set_self_longnick(content).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            InstanceLog.Erro("设置个性签名", e.Message, e.StackTrace);
            return false;
        }
    }
    /// <summary>
    /// 设置个性签名
    /// </summary>
    public async void SetSelfLongnick(string content)
    {
        try {
            await MsgHandle.SendMsg.PostSend(HttpClient, SetSelfLongnickAPI, new set_self_longnick(content).JsonText);
        } catch (Exception e) {
            InstanceLog.Erro("设置个性签名", e.Message, e.StackTrace);
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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, SetQQAvatarAPI, new set_qq_avatar(path).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            InstanceLog.Erro("设置QQ头像", e.Message, e.StackTrace);
            return false;
        }
    }
    /// <summary>
    /// 设置QQ头像
    /// </summary>
    public async void SetQQAvatar(string path)
    {
        try {
            await MsgHandle.SendMsg.PostSend(HttpClient, SetQQAvatarAPI, new set_qq_avatar(path).JsonText);
        } catch (Exception e) {
            InstanceLog.Erro("设置QQ头像", e.Message, e.StackTrace);
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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, SetOnlineStatusAPI, new set_online_status(type).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            InstanceLog.Erro("设置在线状态", e.Message, e.StackTrace);
            return false;
        }
    }
    /// <summary>
    /// 设置在线状态
    /// </summary>
    public async void SetOnlineStatus(set_online_status.OnlineType type)
    {
        try {
            await MsgHandle.SendMsg.PostSend(HttpClient, SetOnlineStatusAPI, new set_online_status(type).JsonText);
        } catch (Exception e) {
            InstanceLog.Erro("设置在线状态", e.Message, e.StackTrace);
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
    public async Task<bool> SetFriendAddRequestAsync(string flag, bool approve, string remark)
    {
        HttpResponseMessage httpc;
        try {
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, SetFriendAddRequestAPI, new set_friend_add_request(flag, approve, remark).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            InstanceLog.Erro("处理好友请求", e.Message, e.StackTrace);
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
            await MsgHandle.SendMsg.PostSend(HttpClient, SetFriendAddRequestAPI, new set_friend_add_request(flag, approve, remark).JsonText);
        } catch (Exception e) {
            InstanceLog.Erro("处理好友请求", e.Message, e.StackTrace);
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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, SendLikeAPI, new send_like(user_id, num).JsonText);
            if (httpc.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
            return false;
        }
    }
    /// <summary>
    /// 给目标用户点赞
    /// </summary>
    public async void SendLike(string user_id, int num)
    {
        try {
            //await SendMesg.Send(SendLikeAPI, new send_like(user_id, num).JsonText);
            await SendLikeAsync(user_id, num);
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
        }
    }

    /// <summary>
    /// 给info中的UserId点赞
    /// </summary>
    public async void SendLike(MsgInfo info, int num) => await SendLikeAsync(info.UserId.ToString(), num);
    #endregion

    #region 获取群列表 get_group_list

    public async Task<List<GroupInfo>?> GetGroupList()
    {
        var requMsg = await PostSend(HttpClient, GetGroupListAPI, "");
        if(requMsg.StatusCode != HttpStatusCode.OK)
            return null;

        var requJson = await requMsg.Content.ReadAsStringAsync();
        requJson.GetJsonElement(out var groupList);
        return groupList.Deserialize<get_group_list>()?.GroupInfos;
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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, GetStrangerInfoAPI, new get_stranger_info(user_id).JsonText);
            if ((int)httpc.StatusCode != 200)
                return null;
            return JsonSerializer.Deserialize<get_stranger_infoReturn>(await httpc.Content.ReadAsStringAsync());
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, GetProFileLikeAPI, "{}");
            if ((int)httpc.StatusCode != 200)
                return null;
            return JsonSerializer.Deserialize<get_profile_like>(await httpc.Content.ReadAsStringAsync());
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, GetFriendsWithCategoryAPI, "{}");
            if ((int)httpc.StatusCode != 200)
                return null;
            return JsonSerializer.Deserialize<get_friends_with_category>(await httpc.Content.ReadAsStringAsync());
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, GetFriendListAPI, new get_friend_list().JsonText);
            if ((int)httpc.StatusCode != 200)
                return null;
            return JsonSerializer.Deserialize<get_friend_listReturn>(await httpc.Content.ReadAsStringAsync());
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, ArkShareGroupAPI, requestJson, null);
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
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
    public async Task<ArkSharePeerReturn?> GetArkSharePeerAsync(string id, ArkSharePeerEnum type)
    {
        HttpResponseMessage? httpc = null;
        try {
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, ArkSharePeerAPI, new ArkSharePeer(id, type).ToString(), null);
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
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
            _ = await MsgHandle.SendMsg.PostSend(HttpClient, CreateCollectionAPI, new create_collection(bried, rowdata).JsonText);
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, CreateCollectionAPI, new create_collection(bried, rowdata).JsonText);
            if ((int)httpc.StatusCode != 200)
                return false;
            return true;
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
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
            _ = await MsgHandle.SendMsg.PostSend(HttpClient, DeleteFriendAPI, new delete_friend(user_id, tempBlock, tempBothDel).JsonText);
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
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
            httpc = await MsgHandle.SendMsg.PostSend(HttpClient, DeleteFriendAPI, new delete_friend(user_id, tempBlock, tempBothDel).JsonText);
            if ((int)httpc.StatusCode != 200)
                return false;
            return true;
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
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
    public async void SendMarkDown(string id, string content, MsgTo type)
    {
        var MarkDownJson = new MarkDownJson(content);
        var 二级转发消息 = new TwoForwardJson(MarkDownJson);
        var 一级转发消息 = new ForwardData(二级转发消息);
        ForwardMsgJson postContent = new ForwardMsgJson(id, 一级转发消息, type);
        string postContents = JsonSerializer.Serialize(postContent);

        string POSTURI = HttpURI + "send_forward_msg";
        HttpResponseMessage? postReturnContent = await MsgHandle.SendMsg.PostSend(HttpClient, POSTURI, postContents);
        await postReturnContent.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 此方法发送的MarkDown 聊天记录消息用户为拉起用户
    /// </summary>
    /// <param name="id"> 群ID / 个人ID </param>
    /// <param name="content"> 内容 </param>
    /// <param name="type"> 类型 </param>
    public async void SendMarkDown(string id, string content, MsgInfo mesg, MsgTo type)
    {
        var MarkDownJson = new MarkDownJson(content);
        var 二级转发消息 = new TwoForwardJson(mesg.UserId.ToString(), mesg.UserName, MarkDownJson);
        var 一级转发消息 = new ForwardData(mesg.UserId.ToString(), mesg.UserName, 二级转发消息);
        ForwardMsgJson postContent = new ForwardMsgJson(id, 一级转发消息, type);
        string postContents = JsonSerializer.Serialize(postContent);

        string POSTURI = HttpURI + "send_forward_msg";
        HttpResponseMessage? postReturnContent = await MsgHandle.SendMsg.PostSend(HttpClient, POSTURI, postContents);
        await postReturnContent.Content.ReadAsStringAsync();
    }

    public async void SendMarkDown(string id, MsgInfo mesg, List<string> markdownContents, MsgTo type)
    {
        List<MsgJson> contents = new List<MsgJson>();
        foreach (var content in markdownContents)
            contents.Add(new MarkDownJson(content));

        var 二级转发消息 = new TwoForwardJson(mesg.UserId, mesg.UserName, contents);
        var 一级转发消息 = new ForwardData(mesg.UserId, mesg.UserName, 二级转发消息);
        ForwardMsgJson postContent = new ForwardMsgJson(id, 一级转发消息, type);
        string postContents = JsonSerializer.Serialize(postContent);

        string POSTURI = HttpURI + "send_forward_msg";
        HttpResponseMessage? postReturnContent = await MsgHandle.SendMsg.PostSend(HttpClient, POSTURI, postContents);
        await postReturnContent.Content.ReadAsStringAsync();
    }

    public void SendMarkDown(string id, MsgInfo mesg, MsgTo type, params string[] markdownContents) => SendMarkDown(id, mesg, markdownContents.ToList(), type);
    #endregion

    #region 合并转发消息
    /// <summary>
    /// 发送，合并转发消息
    /// </summary>
    /// <param name="id"> 目标 </param>
    /// <param name="mesgInfo"></param>
    /// <param name="msgs"></param>
    /// <param name="mesgTo"></param>
    public async void SendForawrd(string id, MsgInfo mesgInfo, IEnumerable<MsgJson> msgs, MsgTo mesgTo)
    {
        List<ForwardData> fd = new List<ForwardData>();
        foreach (var json in msgs) {
            fd.Add(new ForwardData(mesgInfo.UserId.ToString(), mesgInfo.UserName, json));
        }
        var fmj = new ForwardMsgJson(id, fd, mesgTo);
        string RequtContent = JsonSerializer.Serialize(fmj);
        string POSTURI = HttpURI + "send_forward_msg";
        try {
            await MsgHandle.SendMsg.PostSend(HttpClient, POSTURI, RequtContent);
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
        }
    }

    /// <summary>
    /// 发送合并转发消息，根据info中的id决定消息去处
    /// </summary>
    /// <param name="info"> 消息引用 </param>
    /// <param name="msgJsons"> 合并转发消息的内容，没一条就是一条消息 </param>
    public void SendForawrd(MsgInfo info, IEnumerable<MsgJson> msgJsons) => SendForawrd(info.GetId(), info, msgJsons, info.GetMsgTo());
    #endregion

    #region 发送消息
    /// <summary>
    /// 发送单一消息内容
    /// </summary>
    /// <param name="id"> 目标id </param>
    /// <param name="type"> 私聊还是群聊 </param>
    /// <param name="content"> 消息内容 </param>
    public void SendMsg(string id, MsgTo type, MsgJson content) => SendMsg(id, type, new List<MsgJson>() { content });

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="id"> 目标id </param>
    /// <param name="type"> 私聊还是群聊 </param>
    /// <param name="contents"> 消息内容 </param>
    public void SendMsg(string id, MsgTo type, params MsgJson[] contents) => SendMsg(id, type, contents.ToList());

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="id"> 目标id </param>
    /// <param name="type"> 私聊还是群聊 </param>
    /// <param name="contents"> 消息内容 </param>
    public async void SendMsg(string id, MsgTo type, List<MsgJson> contents)
    {
        SendJson postJson = new SendJson(id, contents, type);
        string requestUri = GetMsgSendToURI(type);
        HttpResponseMessage message = await MsgHandle.SendMsg.PostSend(HttpClient, requestUri, postJson.JsonText);
        string content = await message.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 发送消息，根据info中的属性判断消息去向
    /// </summary>
    /// <param name="info"> 消息引用 </param>
    /// <param name="contents"> 消息内容 </param>
    public void SendMsg(MsgInfo info, IEnumerable<MsgJson> contents) => SendMsg(info.GetId(), info.GetMsgTo(), contents.ToList());
    #endregion

    #region 发送图片消息
    /// <summary>
    /// 发送图片消息
    /// </summary>
    /// <param name="id"> 发往的id </param>
    /// <param name="mesgTo"> 是发群里还是私聊 </param>
    /// <param name="filePath"> 文件路径 </param>

    public void SendImage(string id, MsgTo mesgTo, string filePath)
    {
        string fileBase64 = ImageJson.ToBase64(filePath);
        ImageJson img = new ImageJson(fileBase64);
        SendMsg(id, mesgTo, img);
    }

    /// <summary>
    /// 发送图片消息，根据info内属性，判断消息去处
    /// </summary>
    /// <param name="info"> 消息引用 </param>
    /// <param name="filePath"> 文件路径 </param>
    public void SendImage(MsgInfo info, string filePath) => SendImage(info.GetId(), info.GetMsgTo(), filePath);
    #endregion

    #region 群管理

    #region 群禁言 set_group_ban
    /// <summary>
    /// 群禁言，单位秒
    /// </summary>
    public void GroupBan(string groupId, string userId, double time)
    {
        if (!TrueGroupMsg(groupId)) return;
        //GroupBanAPI;
        var json = new set_group_ban(groupId, userId, time);
        _ = PostSend(HttpClient, GroupBanAPI, json.JsonText);
    }

    public void GroupBan(long groupId, long userId, double time) => GroupBan(groupId.ToString(), userId.ToString(), time);
    /// <summary>
    /// 群禁言，info是谁就禁谁。单位 秒
    /// </summary>
    public void GroupBan(MsgInfo info, double time)
    {
        GroupBan(info.GroupId, info.UserId, time);
    }
    #endregion 群禁言

    #region 群踢人 set_group_kick
    /// <summary>
    /// 群踢人 
    /// </summary>
    /// <param name="groupid">群id</param>
    /// <param name="userid">用户id</param>
    public void GroupTick(string groupid, string userid)
    {
        if (!TrueGroupMsg(groupid)) return;
        var json = new set_group_kick(groupid, userid);
        _ = PostSend(HttpClient, GroupKickAPI, json.JsonText);
    }

    public void GroupTick(long groupid, long userid) => GroupTick(groupid.ToString(), userid.ToString());
    /// <summary>
    /// 群踢人，使用info中的信息
    /// </summary>
    public void GroupTick(MsgInfo info)
    {
        GroupTick(info.GroupId, info.UserId);
    }

    #endregion 群踢人
    
    /// <summary>
    /// 有效返回true
    /// </summary>
    private static bool TrueGroupMsg(string groupid)
    {
        if (string.IsNullOrEmpty(groupid))
            return false;
        return true;
    }

    private static bool TrueGroupMsg(long groupid) => TrueGroupMsg(groupid.ToString());
    #endregion

    #region 戳一戳
    /// <summary>
    /// 戳一戳
    /// </summary>
    public void Poke(string userId, int @for = 1)
    {
        var json = new send_poke(userId);
        for(int i = 0; i < @for; i++) {
            PostSend(HttpClient, PoKeAPI, json.JsonText);
        }
    }

    /// <summary>
    /// 群戳一戳
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="for"></param>
    public void Poke(string groupid, string userId, int @for = 1)
    {
        var json = new send_poke(groupid, userId);
        for (int i = 0; i < @for; i++) {
            PostSend(HttpClient, PoKeAPI, json.JsonText);
        }
    }

    public void Poke(long groupid, long userId, int @for = 1) => Poke(groupid.ToString(), userId.ToString(), @for);
    public void Poke(MsgInfo info, int @for = 1)
    {
        if (TrueGroupMsg(info.GroupId)) {
            Poke(info.GroupId, info.UserId, @for);
        } else {
            Poke(info.UserId, @for);
        }
    }
    #endregion
}
