using NapCatScript.Start;

namespace TestPlugin;

public class LikeMe : PluginType
{
    public override void Init()
    {
    }

    public override async Task Run(MesgInfo mesg, string httpUri)
    {
        if ("赞我".Equals(mesg.MessageContent)) {
            for (int i = 0; i < 10; i++) {
                await Task.Delay(10);
                await Send.SendLikeAsync(mesg.UserId, 1);
            }
        }
    }
}
