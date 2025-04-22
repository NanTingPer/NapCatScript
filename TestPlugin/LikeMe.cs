using NapCatScript.Core;

namespace TestPlugin;

public class LikeMe : PluginType
{
    public override void Init()
    {

    }

    public override async Task Run(MsgInfo mesg, string httpUri)
    {
        string mesgContent = mesg.MessageContent.Trim();
        if (mesgContent.StartsWith("#md#")) {
            string makdown = mesgContent.Substring(4);
            var to = Send.GetMesgTo(mesg, out var id);
            Send.SendMarkDown(id, makdown, mesg, to);
        }

        if ("赞我".Equals(mesgContent)) {
            for (int i = 0; i < 10; i++) {
                await Task.Delay(10);
                await Send.SendLikeAsync(mesg.UserId, 1);
            }
        }
    }
}
