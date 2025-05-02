using SQLite;
using System.Threading.Tasks;

namespace KeyWordBan;

public sealed class KeyWordRegister
{
    private static string s_banKeyWordDataPath = Path.Combine(Environment.CurrentDirectory, "BanKeyWord.db");
    private SQLiteAsyncConnection connection;

    public KeyWordRegister()
    {
        connection = new SQLiteAsyncConnection(s_banKeyWordDataPath);
    }

    /// <summary>
    /// 添加，如果存在则更新
    /// </summary>
    /// <param name="keyWordModel"></param>
    /// <returns></returns>
    public async Task<bool> Add(KeyWordModel keyWordModel)
    {
        await connection.CreateTableAsync<KeyWordModel>();
        //var queryList = await connection.QueryAsync<KeyWordModel>("select * from KeyWordTable where keyword = ?", keyWordModel.KeyWord);
        var first = await connection.Table<KeyWordModel>().FirstOrDefaultAsync(f => f.KeyWord == keyWordModel.KeyWord);
        if(first is null) {
            try {
                await connection.InsertAsync(keyWordModel);
            } catch (Exception e){
                Log.Erro(e.Message,e.StackTrace);
                return false;
            }
            return true;
        } else {
            first.Time = keyWordModel.Time;
            try {
                await connection.UpdateAsync(first);
            } catch (Exception e) {
                Log.Erro(e.Message, e.StackTrace);
                return false;
            }
            return true;
        }
    }

    public async Task<bool> Delete(KeyWordModel kwm)
    {
        await connection.CreateTableAsync<KeyWordModel>();
        try {
            await connection.DeleteAsync<KeyWordModel>(kwm.KeyWord);
            return true;
        }catch(Exception e) {
            Log.Erro(e.Message, e.StackTrace);
            return false;
        }
    }

    public async Task<List<KeyWordModel>> GetAll()
    {
        await connection.CreateTableAsync<KeyWordModel>();
        return await connection.Table<KeyWordModel>().Where(f => true).ToListAsync();
    }
}
