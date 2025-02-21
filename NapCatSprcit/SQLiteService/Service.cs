using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapCatSprcit.SQLiteService
{
    public class Service
    {
        private static ISQLiteAsyncConnection _connection;
        private static ISQLiteAsyncConnection Connection
        {
            get
            {
                if (_connection == null)
                    InitSQL();

                return _connection;
            }
            set => _connection = value;
        } 
        private Service() { }

        private static async void InitSQL()
        {
            _connection = new SQLiteAsyncConnection(PublicProperty.DatabasePath);
            await _connection.CreateTableAsync<DataModel>();
        }

        /// <summary>
        /// 获取给定键的内容 可能为空
        /// </summary>
        /// <param name="key"> key </param>
        /// <returns></returns>
        public static async Task<DataModel> GetAsync(string key)
        {
            Console.WriteLine($"获取内容: {key}");
            return await Connection.Table<DataModel>().FirstOrDefaultAsync(f => f.Key == key);
        }

        /// <summary>
        /// 将指定的内容插入数据库 如果存在更新值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task SetAsync(DataModel data) 
        {
            DataModel dm = await GetAsync(data.Key);
            if(dm != null)
            {
                Console.WriteLine($"修改原有数据: {dm.Key} => {dm.Value}");
                dm.Value = data.Value;
                await Connection.UpdateAsync(dm);
            }
            else
            {
                Console.WriteLine($"添加新数据: {data.Key} => {data.Value}");
                await Connection.InsertAsync(data);
            }
        }
    }
}
