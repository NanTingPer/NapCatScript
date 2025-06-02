using NapCatScript.Core;
using NapCatScript.Core.Services;
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

    /// <summary>
    /// 加载插件
    /// </summary>
    /// <param name="ps"></param>
    internal static void LoadPlugin(List<PluginType> ps)
    {
        string pluginDirectory = Path.Combine(Environment.CurrentDirectory, "Plugin");
        if (!Directory.Exists(pluginDirectory))
            Directory.CreateDirectory(pluginDirectory);
        string[] plugins = Directory.GetDirectories(pluginDirectory); //给的是绝对路径
        foreach (var pluginPath in plugins) {
            string pluginName = new DirectoryInfo(pluginPath).Name;
            InstanceLog.Info("加载插件: " + pluginName);
            string pluginDllPath = Path.Combine(pluginPath, pluginName + ".dll");
            PluginLoad plugin = new PluginLoad(pluginDllPath);
            Assembly ass = plugin.LoadFromAssemblyPath(pluginDllPath); //加载插件程序集
            #region 初始化插件的全部PluginType
            IEnumerable<Type> pluginStartTypes = ass.GetTypes().Where(f => typeof(PluginType) == f.BaseType);
            foreach (var pluginStartType in pluginStartTypes) {
                try {
                    ConstructorInfo? pluginConstructor = pluginStartType.GetConstructors().FirstOrDefault(f => f.GetParameters().Length == 0);
                    InstanceLog.Info("创建实例: " + pluginStartType.FullName ?? pluginStartType.FullName + "没有无参构造，无法实例化！");
                    if (pluginConstructor is null) return;
                    PluginType obj = (PluginType)pluginConstructor.Invoke(null);
                    obj.Init();
                    ps.Add(obj);
                } catch (Exception e) {
                    InstanceLog.Erro($"插件{pluginStartType.FullName} 创建失败！", e.Message, e.StackTrace);
                }
            }
            #endregion
        }
    }
}
