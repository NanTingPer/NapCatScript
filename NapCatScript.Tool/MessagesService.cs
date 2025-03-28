namespace NapCatScript.Tool
{
    public class MessagesService
    {
        private static SQLiteService sql { get; } = SQLiteService.Service;
        public static MessagesService MService { get; } = new MessagesService();

        private MessagesService() { }

        //public void SetMessageInfo(MesgInfo)
        //{

        //}
    }
}
