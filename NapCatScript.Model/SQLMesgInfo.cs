using SQLite;

namespace NapCatScript.Model;
/// <summary>
/// 用于数据库使用的MesgInfo类
/// </summary>
public class SQLMesgInfo : MesgInfo
{
    [PrimaryKey]
    public string Key { get; } = string.Empty;
}
