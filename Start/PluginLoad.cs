using System.ComponentModel;
using System.Reflection;
using System.Runtime.Loader;

namespace NapCatScript.Start;
public class PluginLoad : AssemblyLoadContext
{
    private AssemblyDependencyResolver resolver;
    public bool IsActive = true;
    public PluginLoad(string? pluginPath) : base(isCollectible: false)
    {
        resolver = new AssemblyDependencyResolver(pluginPath);//目标程序集路径
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? dllPath = resolver.ResolveAssemblyToPath(assemblyName);
        if (dllPath is null)
            IsActive = false;
        else
            return LoadFromAssemblyPath(dllPath);
        return null;
    }

    /// <summary>
    /// 加载非托管库程序集
    /// </summary>
    protected override nint LoadUnmanagedDll(string unmanagedDllName)
    {
        string? dllPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if(dllPath != null) {
            return LoadUnmanagedDllFromPath(dllPath);
        }
        return nint.Zero;
    }
    //ResolveUnmanagedDllToPath方法会返回此程序集对应的本地路径
    //LoadUnmanagedDllFromPath 方法，从给定路径加载非托管程序集
}
