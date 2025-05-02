namespace KeyWordBan;

[SQLite.Table("KeyWordTable")]
public class KeyWordModel
{
    [SQLite.Column("keyword"), SQLite.PrimaryKey]
    public string KeyWord { get; set; }

    [SQLite.Column("time")]
    public int Time { get; set; } = 1;
}
