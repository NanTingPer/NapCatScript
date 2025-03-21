using SQLite;
namespace NapCatScript.Tool;

/// <summary>
/// 灾厄WIKI别名映射
/// </summary>
public class MapModel
{
    /// <summary>
    /// Key是newString
    /// </summary>
    [Column("new"), PrimaryKey]
    public string Key { get; set; } = "";

    [Column("old")]
    public string oldString { get; set; } = "";

    [Column("userid")]
    public string UserId { get; set; } = "";

    [Column("createtime")]
    public string CreateTime { get; set; } = "";
}
