namespace NapCatScript.JsonFromat;

/// <summary>
/// 已有:
///     Json        Json卡片
///     Profile     设置个人消息
///     Image       图片消息
///     Video       视频消息
///     Record      语音消息
///     At          At消息
///     setonline   设置在线状态
///     ArkShare    获取群聊卡片
///     ArkShare    获取推荐好友
///     https://napcat.apifox.cn/226659197e0 获取点赞列表
/// </summary>
public abstract class BaseMesg
{
    public abstract string JsonText { get; set; }
    public virtual string GetString()
    {
        return JsonText;
    }

    public override string ToString()
    {
        return JsonText;
    }
}


