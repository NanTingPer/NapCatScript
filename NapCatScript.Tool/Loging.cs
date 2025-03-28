﻿namespace NapCatScript.Tool;

public class Loging
{
    private static string logPath = Path.Combine(Environment.CurrentDirectory, "log.log");
    private static StreamWriter Writer { get; set; }
    static Loging()
    {
        Writer = File.CreateText(logPath);
    }

    public static Loging Log { get; set; } = new Loging();

    public void Info(params object[] content)
    {
        foreach (var obj in content) {
            Writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " Info :" + obj.ToString());
        }
        Writer.Flush();
    }

    public void Waring(params object[] content)
    {
        foreach (var obj in content) {
            Writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " Waring :" + obj.ToString());
        }
        Writer.Flush();
    }

    public void Erro(params object[] content)
    {
        foreach (var obj in content) {
            Writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " Erro :" + obj.ToString());
        }
        Writer.Flush();
    }

}
