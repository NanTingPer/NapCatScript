using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapCatScript.Tool
{
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
    }
}