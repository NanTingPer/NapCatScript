using SQLite;
using System.Linq.Expressions;
using Key = System.Reflection.PropertyInfo;

namespace NapCatScript.Core.Services;

public class SQLiteService
{
    private static readonly string DataBasePath = Path.Combine(Environment.CurrentDirectory, "data.db");

    /// <summary>
    /// 公共数据库
    /// </summary>
    public static SQLiteService SQLService { get; } = new SQLiteService();
    public SQLiteAsyncConnection Connection;
    private SQLiteService()
    {
        Connection = new SQLiteAsyncConnection(DataBasePath);
    }

    /// <summary>
    /// Path是基于运行目录的相对路径
    /// </summary>
    /// <param name="dataBasePath"></param>
    public SQLiteService(string dataBasePath)
    {
        string path = Path.Combine(Environment.CurrentDirectory, dataBasePath);
        string directorPath = Path.GetDirectoryName(path);
        if(!Directory.Exists(directorPath))
            Directory.CreateDirectory(directorPath);
        
        Connection = new SQLiteAsyncConnection(path);
    }

    /// <summary>
    /// <para> 使用Key获取值,这个Key是给定类型代表数据库数据的Key </para> 
    /// <para> 如果给定类型的数据库不存在，会创建，如果不存在返回 default </para>
    /// </summary>
    public async Task<T?> Get<T>(object primaryKey) where T : new()
    {
        await CreateTable<T>();
        return await Connection.GetAsync<T>(primaryKey);
    }

    public async Task<T?> TestGet<T>(object primaryKey) where T : new()
    {
        return await Connection.GetAsync<T>(primaryKey);
    }

    public async Task CreateTable<T>() where T : new()
    {
        await Connection.CreateTableAsync<T>();
    }

    public async Task Update<T>(T data, string keyName = "Key") where T : new()
    {
        await CreateTable<T>();
        Type dataType = typeof(T);
        Key keyInfo = dataType.GetProperty(keyName)!;
        if (keyInfo == null) return;
        var keyValue = keyInfo.GetValue(data);
        if (keyValue is null) return;

        T? oldData;
        try {
            oldData = await Get<T?>(keyValue!.ToString());
        } catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
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
        await CreateTable<T>();
        await Connection.DeleteAsync<T>(key);
    }
    public async Task DeleteALL<T>() where T : new()
    {
        await CreateTable<T>();
        await Connection.DeleteAllAsync<T>();
    }

    public async Task DeleteRange<T>(List<T> delectObj, string keyName = "Key") where T : new()
    {
        var propty = typeof(T).GetProperty(keyName);
        if (propty == null)
            return;
        await CreateTable<T>();
        foreach (var obj in delectObj) {
            try {
                await Delete<T>(propty.GetValue(obj));
            } catch (Exception e) {
                InstanceLog.Erro(e.Message, e.StackTrace);
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

    public async Task Insert<T>(T obj, string keyName = "Key") where T : new()
    {
        try {
            await CreateTable<T>();
            Type type = typeof(T);
            var pros = type.GetProperties();
            var keyProperty = typeof(T).GetProperty(keyName);
            if (keyProperty == null) {
                return;
            }
            var keyValue = keyProperty.GetValue(obj);
            var existing = await Connection.FindAsync<T>(keyValue);
            if (existing == null) {
                await Connection.InsertAsync(obj);
            } else {
                await Update(obj);
            }
        } catch (Exception ex) {
            InstanceLog.Erro(ex.Message, ex.StackTrace);
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
