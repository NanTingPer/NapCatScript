using NapCatScript.Core.JsonFormat;
using NapCatScript.Core.JsonFormat.Msgs;

namespace NapCatScript.Core.JsonFormat;


/// <summary>
/// 内容
/// <para>文本消息 <see cref="TextJson"/> </para>
/// <para>表情消息 <see cref="未定义"/></para>
/// <para>图片消息 <see cref="ImageJson"/></para>
/// <para>回复消息 <see cref="未定义"/></para>
/// <para>Json消息 <see cref="JsonJson"/></para>
/// <para>视频消息 <see cref="VideoJson"/></para>
/// <para>文件消息 <see cref="未定义"/></para>
/// <para>markdown消息 <see cref="MarkDownJson"/></para>
/// <para>发送forward <see cref="ForwardMsgJson"/></para>
/// <para>二级合并转发消息 <see cref="TwoForwardJson"/></para>
/// </summary>
[JsonDerivedType(typeof(TextJson))]
[JsonDerivedType(typeof(ImageJson))]
[JsonDerivedType(typeof(JsonJson))]
[JsonDerivedType(typeof(VideoJson))]
[JsonDerivedType(typeof(TwoForwardJson))]
[JsonDerivedType(typeof(MarkDownJson))]
[JsonDerivedType(typeof(AtJson))]
public abstract class MsgJson
{
    [JsonIgnore]
    public virtual string JsonText { get => JsonSerializer.Serialize(this); }
}
