using SQLite;

namespace NapCatScript.Core.Model;
/// <summary>
/// 用于数据库使用的MesgInfo类
/// </summary>
public class SQLMesgInfo : MsgInfo
{
    [PrimaryKey]
    public string Key { get; set; } = string.Empty;
}
