using NapCatScript.MesgHandle;

namespace NapCatScript.Start;

public abstract class PluginType
{
    public Send Send { get => Main_.SendObject; }
    public abstract void Init();
    public abstract Task Run(MesgInfo mesg, string httpUri);
}
