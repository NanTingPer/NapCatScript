using SQLite;

namespace NapCatScript.Core.Model;

public class FAQModel
{
    [Column("F"), PrimaryKey]
    public string Key { get; set; } = "";

    [Column("Q")]
    public string Value { get; set; } = "";

    [Column("userid")]
    public string UserId { get; set; } = "";

    [Column("username")]
    public string UserName { get; set; } = "";

    [Column("createtime")]
    public string CreateTime { get; set; } = "";
}
