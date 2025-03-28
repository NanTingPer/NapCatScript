namespace NapCatScript.Services;

public class MessagesService
{
    private static SQLiteService sql { get; } = SQLiteService.Service;
    public static MessagesService MService { get; } = new MessagesService();

    private MessagesService() { }

    public void Set(MesgInfo mesg)
    {
        
    }
}

    //    public static SQLMesgInfo ToMesgInfo(this MesgInfo mesg)
    //{
    //}
