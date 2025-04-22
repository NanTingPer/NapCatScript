using NapCatScript.Core.Services;
using NapCatScript.Core.MsgHandle;

namespace NapCatScript.Start;

public abstract class PluginType
{
    protected Loging Log = Loging.Log;
    public Send Send { get => Main_.SendObject; }
    public abstract void Init();
    public abstract Task Run(MesgInfo mesg, string httpUri);
}
