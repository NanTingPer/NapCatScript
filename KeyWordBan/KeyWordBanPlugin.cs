using NapCatScript.Core;

namespace KeyWordBan;

public class KeyWordBanPlugin : PluginType
{
    private Dictionary<string, int> dicWords = [];
    private List<KeyWordModel> listWords = [];
    private KeyWordRegister register = new();
    public override async void Init()
    {
        listWords = await register.GetAll();
        foreach (var item in listWords) {
            dicWords.Add(item.KeyWord, item.Time);
        }
    }

    public override async Task Run(MsgInfo msg, string httpUri)
    {
        //var msgText = msg.MessageContent.;
        var msgText = msg.MessageContent.DelWhiteSpace();
        if (msgText.StartsWith("添加关键字")) {
            if (msgText.GetWordAndTime(out string word, out int time)) {
                await register.Add(new KeyWordModel() { KeyWord = word, Time = time});
                SendTextAsync(msg, httpUri, "添加: " + word + ", " + "时长: " + time + "秒", new CancellationToken());
                return;
            }
        } else if(msgText.StartsWith("删除关键字")) {
            if(msgText.IsDelKeyWord(out var kw)) {
                await register.Delete(kw);
                try {
                    listWords.Remove(listWords.FirstOrDefault(f => f.KeyWord == kw.KeyWord));
                } catch(Exception e) {
                    SendTextAsync(msg, httpUri, "失败！" + e.Message, new CancellationToken());
                    return;
                }
                SendTextAsync(msg, httpUri, "删除: " + kw.KeyWord, new CancellationToken());
                return;
            }
        } else if(msgText == "查看全部关键字#") {
            var k = await register.GetAll();
            var sb = new StringBuilder();
            k.ForEach(f => sb.Append(f.KeyWord + "    " + f.Time + "s"));
            Send.SendForawrd(msg, [new TextJson(sb.ToString())]);
            return;
        }

        foreach (var item in listWords) {
            if (msgText.Contains(item.KeyWord)) {
                Send.GroupBan(msg, item.Time);
                break;
            }
        }
    }
}

public static class Med
{
    public static string DelWhiteSpace(this string msg)
    {
        var sbuild = new StringBuilder();
        foreach (var ch in msg) {
            if (!char.IsWhiteSpace(ch)) sbuild.Append(ch);
        }
        return sbuild.ToString();
    }

    public static bool GetWordAndTime(this string msg, out string word, out int time)
    {
        //添加关键字#c#1
        string[] wt = msg.Split('#');
        word = "";
        time = 0;
        if (wt.Length != 3)
            return false;

        word = wt[1];
        if (int.TryParse(wt[2], out int value)) {
            time = value;
            return true;
        }
        return false;

    }

    public static bool IsDelKeyWord(this string msg, out KeyWordModel? kw)
    {
        //删除关键字#1
        string[] wt = msg.Split('#');
        kw = null;
        if (wt.Length == 2) {
            kw = new KeyWordModel() { KeyWord = wt[1] };
            return true;
        }
        return false;
    }
}
