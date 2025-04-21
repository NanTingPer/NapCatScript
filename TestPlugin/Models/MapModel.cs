using SQLite;
namespace TestPlugin.Models;

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

    /// <summary>
    /// 原本的内容, 比如神明吞噬者
    /// </summary>
    [Column("old")]
    public string oldString { get; set; } = "";

    [Column("userid")]
    public string UserId { get; set; } = "";

    [Column("username")]
    public string UserName { get; set; } = "";

    [Column("createtime")]
    public string CreateTime { get; set; } = "";
}
