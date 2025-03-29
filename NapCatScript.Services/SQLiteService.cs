using SQLite;
using System.Linq.Expressions;
using Key = System.Reflection.PropertyInfo;

namespace NapCatScript.Services;

public class SQLiteService
{
    private static string DataBasePath = Path.Combine(Environment.CurrentDirectory, "data.db");
    public static SQLiteService Service { get; } = new SQLiteService();
    public SQLiteAsyncConnection Connection;
    private SQLiteService()
    {
        Connection = new SQLiteAsyncConnection(DataBasePath);
    }

    /// <summary>
    /// <para> 使用Key获取值,这个Key是给定类型代表数据库数据的Key </para> 
    /// <para> 如果给定类型的数据库不存在，会创建，如果不存在返回 default </para>
    /// </summary>
    public async Task<T?> Get<T>(object key) where T : new()
    {
        await CreateTable<T>();
        if (typeof(T).GetProperty("Key") == null)
            return default;
        return await Connection.GetAsync<T>(key);
    }

    public async Task CreateTable<T>() where T : new()
    {
        await Connection.CreateTableAsync<T>();
    }

    public async Task Update<T>(T data) where T : new()
    {
        await CreateTable<T>();
        Type dataType = typeof(T);
        Key keyInfo = dataType.GetProperty("Key")!;
        if (keyInfo == null) return;
        var keyValue = keyInfo.GetValue(data);
        if (keyValue is null) return;

        T oldData;
        try {
            oldData = await Get<T>(keyValue.ToString());
        } catch (Exception e) {
            Console.WriteLine("没有此数据");
            return;
        }

        Key[] pInfos = dataType.GetProperties();
        foreach (var pinfo in pInfos) {
            var newValue = pinfo.GetValue(data);
            pinfo.SetValue(oldData, newValue);
        }
        await Connection.UpdateAsync(oldData, dataType);
    }

    public async Task Delete<T>(object key) where T : new()
    {
        var propty = typeof(T).GetProperty("Key");
        if (propty == null)
            return;
        await CreateTable<T>();
        await Connection.DeleteAsync<T>(key);
    }
    public async Task DeleteALL<T>() where T : new()
    {
        var propty = typeof(T).GetProperty("Key");
        if (propty == null)
            return;
        await CreateTable<T>();
        await Connection.DeleteAllAsync<T>();
    }

    public async Task DeleteRange<T>(List<T> delectObj) where T : new()
    {
        var propty = typeof(T).GetProperty("Key");
        if (propty == null)
            return;
        await CreateTable<T>();
        foreach (var obj in delectObj) {
            try {
                await Delete<T>(propty.GetValue(obj));
            } catch (Exception e) {
                Console.WriteLine("删除失败:  " + e.Message);
            }
        }
    }

    public async Task<List<T>> GetAll<T>() where T : new()
    {
        await CreateTable<T>();
        return await Connection.Table<T>().ToListAsync();
    }

    public async Task<List<T>> Get<T>(Expression<Func<T, bool>> expr) where T : new()
    {
        await CreateTable<T>();
        return await Connection.Table<T>().Where(expr).ToListAsync();
    }

    public async Task Insert<T>(T obj) where T : new()
    {
        try {
            await CreateTable<T>();
            var keyProperty = typeof(T).GetProperty("Key");
            if (keyProperty == null) {
                Console.WriteLine("未找到Key属性");
                return;
            }
            var keyValue = keyProperty.GetValue(obj);
            var existing = await Connection.FindAsync<T>(keyValue);
            if (existing == null) {
                await Connection.InsertAsync(obj);
                Console.WriteLine("插入成功");
            } else {
                Console.WriteLine("已存在");
            }
        } catch (Exception ex) {
            Console.WriteLine($"插入失败: {ex.Message}");
        }

        //try {
        //    await CreateTable<T>();
        //} catch {
        //    Console.WriteLine("创表失败");
        //    return;
        //}
        //try {
        //    await Connection.GetAsync<T>(obj);
        //} catch {
        //    await Connection.InsertAsync(obj, typeof(T));
        //}
    }
}
