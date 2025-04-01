namespace NapCatScript.MesgHandle.Get;

public class Method
{
    /// <summary>
    /// 获取推荐好友 / 群聊卡片
    /// </summary>
    public string ArkSharePeer { get; } = nameof(ArkSharePeer);

    public JsonElement ArkSharePeerMethod();

}
