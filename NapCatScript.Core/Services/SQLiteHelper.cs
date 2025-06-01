using System.Linq.Expressions;
using System.Reflection;
using SQLite;

using Column = System.Reflection.PropertyInfo;

namespace NapCatScript.Core.Services;

public class SQLiteHelper<TModel> where TModel : new() 
{
    public SQLiteAsyncConnection SQLite { get; set; }
    
    public StringBuilder QuerySQL = new StringBuilder();
    public Column[]? Columns { get; private set; }

    /// <summary>
    /// 基于程序运行目录的相对路径 使用<see cref="Path"/>
    /// </summary>
    /// <param name="dataBasePath"></param>
    public SQLiteHelper(string dataBasePath)
    {
        string path = Path.Combine(Environment.CurrentDirectory, dataBasePath);
        SQLite = new SQLiteAsyncConnection(path);
    }

    public async Task<List<TModel>?> QueryAsync(string tableName, string query, params object[] param)
    {
        var cols = await SQLite.GetTableInfoAsync(tableName);
        if(cols.Count == 0)
            await CreateTableAsync(tableName);

        //TableMapping? table = null;
        //foreach (var mapping in SQLite.TableMappings) {
        //    if (mapping.TableName == tableName) {
        //        table = mapping;
        //        break;
        //    }
        //}
        //if(table is null) return null;
        
        //var sql = new StringBuilder();
        //sql.Append($" SELECT * FROM {tableName} ");

        return await SQLite.QueryAsync<TModel>(query, param);
    }

    public async Task InsertAsync(string tableName, TModel model)
    {
        var cols = await SQLite.GetTableInfoAsync(tableName);
        if(cols.Count == 0)
            await CreateTableAsync(tableName);
        
        var sql = new StringBuilder($" INSERT INTO {tableName} ");
        var valueBuild = new StringBuilder();
        var colBuild = new StringBuilder();
        List<object> par = new List<object>();
        valueBuild.Append(" VALUES ( ");
        colBuild.Append(" ( ");

        int lenght = Columns?.Length ?? 0;
        int currIndex = 0;
        foreach (var col in Columns!) {
            currIndex++;
            
            if(IsKey(col, out var isauto) && isauto)
                continue;
            
            object? value = col.GetValue(model);
            if(value is null)
                continue;
            valueBuild.Append($" ? ");
            par.Add(value);
            colBuild.Append($" {GetColName(col) ?? col.Name} ");
            if (currIndex < lenght) {
                valueBuild.Append(" , ");
                colBuild.Append(" , ");
            }
        }
       
        valueBuild.Append(" ) ");
        colBuild.Append(" ) ");
        sql.Append(colBuild.ToString());
        sql.Append(valueBuild.ToString());
        sql.Append(" ; ");
        await SQLite.ExecuteAsync(sql.ToString(), par.ToArray());
    }
    
    /// <summary>
    /// 创建表
    /// </summary>
    /// <param name="tableName"></param>
    public async Task CreateTableAsync(string tableName)
    {
        var BIP = BindingFlags.Instance | BindingFlags.Public;
        Type tableModel = typeof(TModel);
        Columns = tableModel.GetProperties(BIP);

        var createTableSql = new StringBuilder();
        createTableSql.Append($"CREATE TABLE IF NOT EXISTS {tableName} (");
        int lenght = Columns.Length;
        int currIndex = 0;
        foreach (var info in Columns) {
            currIndex++;
            var isKey = IsKey(info, out bool isAutoIncrement);
            string type = isAutoIncrement ? "INTEGER" : GetSqlType(info.PropertyType);
            
            createTableSql.Append($" {GetColName(info) ?? info.Name} {type} ");
            if(isKey)
                createTableSql.Append(" PRIMARY KEY ");
            if(isAutoIncrement)
                createTableSql.Append(" AUTOINCREMENT ");
            if(currIndex < lenght)
                createTableSql.Append(",");
        }
        createTableSql.Append(")");
        await SQLite.ExecuteAsync(createTableSql.ToString());
    }

    ~SQLiteHelper()
    {
        _ = SQLite.CloseAsync();
    }
    
    private bool IsKey(Column info, out bool isAutoIncrement)
    {
        isAutoIncrement = info.GetCustomAttribute(typeof(AutoIncrementAttribute), false) != null;
        return info.GetCustomAttribute(typeof(PrimaryKeyAttribute), false) != null;
    }

    private string? GetColName(Column info)
    {
        ColumnAttribute? column = (ColumnAttribute?)info.GetCustomAttribute(typeof(ColumnAttribute), false);
        if(column == null) return null;

        if (string.IsNullOrEmpty(column.Name))
            return null;
        
        return column.Name;
    }
    
    private string GetSqlType(Type type)
    {
        if (type == typeof(int) || type == typeof(long) || type == typeof(short))
            return "INTEGER";
        if (type == typeof(float) || type == typeof(double))
            return "REAL";
        if (type == typeof(bool))
            return "INTEGER"; 
        if (type == typeof(string))
            return "TEXT";
        if (type == typeof(DateTime))
            return "TEXT"; 
        if (type == typeof(byte[]))
            return "BLOB";
        return "TEXT"; 
    }
}