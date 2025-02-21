using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapCatSprcit.SQLiteService
{
    /// <summary>
    /// 消息库的格式
    /// </summary>
    public class DataModel
    {
        [SQLite.Column("OneKey"),PrimaryKey,AutoIncrement]
        public long id { get; set; }

        /// <summary>
        /// 消息主唯一标识符
        /// </summary>
        [Column("Key")]
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// 消息内容
        /// </summary>
        [Column("Value")]
        public string Value { get; set; } = string.Empty;
    }
}
