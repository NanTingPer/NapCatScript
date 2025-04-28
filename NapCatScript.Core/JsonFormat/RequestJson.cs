namespace NapCatScript.Core.JsonFormat;

/// <summary>
/// 非消息Json，例如获取群卡片
/// </summary>
public abstract class RequestJson
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
