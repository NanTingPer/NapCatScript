using SQLite;

namespace NapCatScript.Core.Model;

public class DeepSeekModel
{
    [Column("key"), PrimaryKey, AutoIncrement]
    public long Key { get; set; }
    [Column("userid")]
    public string UserId { get; set; } = string.Empty;
    [Column("username")]
    public string UserName { get; set; } = string.Empty;
    [Column("content")]
    public string Content { get; set; } = string.Empty;
    [Column("mesgtype")]
    public string MesgType { get; set; } = string.Empty;
    [Column("groupid")]
    public string GroupId { get; set; } = string.Empty;
}