using SQLite;

namespace NapCatScript.Core.Model;
/// <summary>
/// 用于数据库使用的MesgInfo类
/// </summary>
public class SQLMesgInfo : MsgInfo
{
    public readonly static string KeyName = nameof(SQLMesgKey);
    [PrimaryKey]
    public string SQLMesgKey { get; set; } = string.Empty;
}
