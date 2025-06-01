namespace NapCatScript.Core.Services;

public class Log
{
    private static string logPath = Path.Combine(Environment.CurrentDirectory, "log.log");
    private static StreamWriter Writer { get; set; }
    static Log()
    {
        Writer = File.CreateText(logPath);
    }

    public static Log InstanceLog { get; set; } = new Log();

    public void Info(params object?[] content)
    {
        foreach (var obj in content) {
            if (obj is null)
                continue;
            string logContent = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " Info :" + obj.ToString();
            Writer.WriteLine(logContent);
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(logContent);
            Console.ForegroundColor = tempColor;
        }
        Writer.Flush();
    }

    public void Waring(params object?[] content)
    {
        foreach (var obj in content) {
            if (obj is null)
                continue;
            string logContent = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " Waring :" + obj.ToString();
            Writer.WriteLine(logContent);
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(logContent);
            Console.ForegroundColor = tempColor;
        }
        Writer.Flush();
    }

    public void Erro(params object?[] content)
    {
        foreach (var obj in content) {
            if (obj is null)
                continue;
            Writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " Erro :" + obj.ToString());
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(obj.ToString());
            Console.ForegroundColor = tempColor;
        }
        Writer.Flush();
    }

}
