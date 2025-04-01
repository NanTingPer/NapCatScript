namespace NapCatScript.JsonFromat.Mesgs;

/// <summary>
/// 已有:
///     Json        Json卡片
///     Profile     设置个人消息
///     Image       图片消息
///     Video       视频消息
///     Record      语音消息
///     At          At消息
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


