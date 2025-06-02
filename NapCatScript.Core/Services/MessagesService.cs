using System.Linq.Expressions;

namespace NapCatScript.Core.Services;

/*public class MessagesService
{
    private static SQLiteService sql { get; } = SQLiteService.SQLService;
    public static MessagesService MService { get; } = new MessagesService();

    private MessagesService() { }

    public async void SetAsync(MsgInfo mesg)
    {
        await sql.Insert(mesg.ToMesgInfo(), SQLMesgInfo.KeyName);
    }

    public async Task<List<SQLMesgInfo>> Get(Expression<Func<SQLMesgInfo, bool>> expr)
    {
        return await sql.Get(expr);
    }
}
*/
static class MesgExt
{
    public static SQLMesgInfo ToMesgInfo(this MsgInfo mesg)
    {
        var obj = new SQLMesgInfo();
        obj.SQLMesgKey = Guid.NewGuid().ToString("N");
        TypeMap(typeof(MsgInfo), typeof(SQLMesgInfo), mesg, obj);
        return obj;
    }
}
