namespace NapCatScript.Core.JsonFormat;
/// <summary>
/// 消息类型的枚举值
/// <para> 群聊 私聊通用 </para>
/// </summary>
public enum MsgType
{   
    /// <summary>
    /// 文本
    /// </summary>
    text,

    /// <summary>
    /// 艾特
    /// </summary>
    at,

    /// <summary>
    /// 图片
    /// </summary>
    image,

    /// <summary>
    /// 系统表情
    /// </summary>
    face,

    /// <summary>
    /// json
    /// </summary>
    json,

    /// <summary>
    /// 语音
    /// </summary>
    record,

    /// <summary>
    /// 视频
    /// </summary>
    video,

    /// <summary>
    /// 回复 需要下一个需要使用text
    /// </summary>
    reply,

    /// <summary>
    /// 音乐卡片
    /// </summary>
    music,

    /// <summary>
    /// 超级表情 骰子
    /// </summary>
    dice,

    /// <summary>
    /// 超级标签 猜拳
    /// </summary>
    rps,

    /// <summary>
    /// 合并
    /// </summary>
    node,
}

public enum MsgTo
{
    group,
    user
}