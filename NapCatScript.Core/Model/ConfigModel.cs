using SQLite;

namespace NapCatScript.Core.Model;

/// <summary>
/// 配置项
/// </summary>
public class ConfigModel
{
    /// <summary>
    /// 配置名称
    /// </summary>
    [Column("name"), PrimaryKey]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 配置内容
    /// </summary>
    [Column("content")]
    public string Content { get; set; } = string.Empty;

}
