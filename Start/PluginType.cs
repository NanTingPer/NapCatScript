﻿using NapCatScript.MesgHandle;
using NapCatScript.Services;

namespace NapCatScript.Start;

public abstract class PluginType
{
    protected Loging Log = Loging.Log;
    public Send Send { get => Main_.SendObject; }
    public abstract void Init();
    public abstract Task Run(MesgInfo mesg, string httpUri);
}
