using NapCatScript.JsonFromat.Mesgs;

namespace NapCatScript.JsonFromat;


/// <summary>
/// 内容
/// <para>文本消息<see cref="Mesgs.TextMsgJson"/> </para>
/// <para>表情消息<see cref="未定义"/></para>
/// <para>图片消息<see cref="未定义"/></para>
/// <para>回复消息<see cref="未定义"/></para>
/// <para>Json消息<see cref="Mesgs.JsonMsgJson"/></para>
/// <para>视频消息<see cref="Mesgs.VideoMsgJson"/></para>
/// <para>文件消息<see cref="未定义"/></para>
/// <para>markdown消息<see cref="MarkDownJson"/></para>
/// <para>发送forward<see cref="未定义"/></para>
/// <para>二级合并转发消息<see cref="Mesgs.TwoForwardMsgJson"/></para>
/// </summary>
[JsonDerivedType(typeof(TextMsgJson))]
[JsonDerivedType(typeof(JsonMsgJson))]
[JsonDerivedType(typeof(VideoMsgJson))]
[JsonDerivedType(typeof(TwoForwardMsgJson))]
[JsonDerivedType(typeof(MarkDownJson))]
[JsonDerivedType(typeof(AtMsgJson))]
public abstract class MsgJson
{
    [JsonIgnore]
    public abstract string JsonText { get; set; }
}
