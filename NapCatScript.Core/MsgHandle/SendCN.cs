namespace NapCatScript.Core.MsgHandle;
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
    public void 发送MarkDown(string 目标, string 内容, MsgInfo 消息引用, MsgTo 去处) => send.SendMarkDown(目标, 内容, 消息引用, 去处);
    /// <summary>
    /// 此方法发送的MarkDown用户id为123456. 昵称为 匿名
    /// </summary>
    /// <param name="id"> 群ID / 个人ID </param>
    /// <param name="content"> 内容 </param>
    /// <param name="type"> 类型 </param>
    public void 发送MarkDown(string 目标, string 内容, MsgTo 去处) => send.SendMarkDown(目标, 内容, 去处);
    public void 发送MarkDown(string 目标, MsgInfo 来源的方法引用, MsgTo 去处, params string[] 内容集) => send.SendMarkDown(目标, 来源的方法引用, 去处, 内容集);
    public void 发送MarkDown(string 目标, MsgInfo 来源的方法引用, List<string> 内容集, MsgTo 去处) => send.SendMarkDown(目标, 来源的方法引用, 内容集, 去处);
}

