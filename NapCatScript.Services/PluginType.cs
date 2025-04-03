namespace NapCatScript.Services;

public abstract class PluginType
{
    public abstract void Init();
    public abstract void Run(MesgInfo mesg, string httpUri);
}
