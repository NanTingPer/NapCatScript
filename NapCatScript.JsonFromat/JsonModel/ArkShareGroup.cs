namespace NapCatScript.JsonFromat.JsonModel;

/// <summary>
/// 获取推荐群聊卡片Json
/// </summary>
public class ArkShareGroup(string group_id) : BaseMesg //获取推荐群聊卡片Json
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(group_id));

    public class Root(string group_id)
    {
        public string Group_id { get; set; } = group_id;
    }
}
