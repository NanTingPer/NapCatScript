using NapCatScript.Core.Services;
using NapCatScript.Core.MsgHandle;

namespace NapCatScript.Core;

public abstract class PluginType
{
    protected Log Log = Log.InstanceLog;
    public Send Send { get; } = CoreConfigValueAndObject.SendObject;
    public virtual void Init() { }
    public abstract Task Run(MsgInfo msg, string httpUri);
}
