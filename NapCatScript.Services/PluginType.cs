namespace NapCatScript.Services;

public abstract class PluginType
{
    public abstract void Init();
    public abstract Task Run(MesgInfo mesg, string httpUri);
}
