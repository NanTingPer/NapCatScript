namespace NapCatScript.Core.MsgHandle;
/// <summary>
/// <para> https://napcat.apifox.cn/226799128e0 </para>
/// <para> 全部发送私聊消息 </para> 
/// <para> 全部发送群聊消息 </para>
/// </summary>
public static class API
{
    #region 发送私聊消息
    /// <summary>
    /// <para> 私聊文本 </para> 
    /// <para> 私聊图片 </para> 
    /// <para> 私聊系统表情 </para> 
    /// <para> 私聊JSON </para> 
    /// <para> 私聊语音 </para> 
    /// <para> 私聊视频 </para> 
    /// <para> 私聊回复 </para> 
    /// <para> 私聊音乐卡片 </para> 
    /// <para> 私聊自定义音乐卡片 </para> 
    /// <para> 私聊超级表情 骰子 </para> 
    /// <para> 私聊超级表情 猜拳 </para> 
    /// <para> 私聊文件 </para>
    /// <para> /send_private_msg </para>
    /// <para> Body 参数application/json </para>
    /// </summary>
    public const string PrivateMsg = "/send_private_msg";
    public const string PrivateMsgNoX = "send_private_msg";

    /// <summary>
    /// <para> 私聊合并转发消息 </para> 
    /// <para> /send_private_forward_msg </para>
    /// <para> Body 参数application/json </para>
    /// </summary>
    public const string PrivateForward = "/send_private_forward_msg";
    public const string PrivateForwardNoX = "send_private_forward_msg";

    /// <summary>
    /// <para> 消息转发到私聊 </para> 
    /// <para> /forward_friend_single_msg </para> 
    /// <para> Body 参数application/json </para>
    /// </summary>
    public const string PrivateFriendSingle = "/forward_friend_single_msg";
    public const string PrivateFriendSingleNoX = "forward_friend_single_msg";

    /// <summary>
    /// <para> 私聊戳一戳 </para> 
    /// <para> /friend_poke </para>  
    /// <para> Body 参数application/json </para>
    /// </summary>
    public const string PrivateFriendPoke = "/friend_poke";
    public const string PrivateFriendPokeNoX = "friend_poke";
    #endregion

    #region 发送群聊消息
    /// <summary>
    /// <para> 发送群文本 </para>
    /// <para> 发送群艾特 </para>
    /// <para> 发送群图片 </para>
    /// <para> 发送群语音 </para>
    /// <para> 发送群视频 </para>
    /// <para> 发送群回复 </para>
    /// <para> 发送群文件 </para>
    /// <para> 发送群JSON </para>
    /// <para> 发送群系统表情 </para>
    /// <para> 发送群聊音乐卡片 </para>
    /// <para> 群聊自定义音乐卡片 </para>
    /// <para> 发送群聊超级表情 骰子 </para>
    /// <para> 发送群聊超级表情 猜拳 </para>
    /// <para> /send_group_msg </para>
    /// <para> Body 参数application/json </para>
    /// </summary>
    public const string GroupMsg = "/send_group_msg";
    public const string GroupMsgNoX = "send_group_msg";

    /// <summary>
    /// 发送群合并转发消息
    /// <para> Body 参数application/json </para>
    /// </summary>
    public const string GroupMsgForward = "/send_group_forward_msg";
    public const string GroupMsgForwardNoX = "send_group_forward_msg";

    /// <summary>
    /// 消息转发到群
    /// <para> /forward_group_single_msg </para>
    /// <para> Body 参数application/json </para>
    /// </summary>
    public const string GroupMsgSingle = "/forward_group_single_msg";
    public const string GroupMsgSingleNoX = "forward_group_single_msg";

    /// <summary>
    /// 群聊戳一戳
    /// <para> /group_poke </para>
    /// <para> Body 参数application/json </para>
    /// </summary>
    public const string GroupMsgPoke = "/group_poke";
    public const string GroupMsgPokeNoX = "group_poke";
    #endregion
}
