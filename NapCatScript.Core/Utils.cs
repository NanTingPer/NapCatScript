using System.Reflection;

namespace NapCatScript.Core;

public static class Utils
{
    /// <summary>
    /// 将viewModelObject的值赋值给serverObject
    /// </summary>
    /// <param name="thisType"> viewModel类型 </param>
    /// <param name="serverType"> server类型 </param>
    /// <param name="viewModelObject"> viewModel对象 </param>
    /// <param name="serverObject"> server对象 </param>
    public static void TypeMap(Type thisType, Type serverType, object viewModelObject, object serverObject)
    {
        var BIP = BindingFlags.Instance | BindingFlags.Public;
        IEnumerable<PropertyInfo> viewModelInfos = thisType.GetProperties(BIP);
        HashSet<string> proName = [];
        foreach (var propertyInfo in viewModelInfos) {
            proName.Add(propertyInfo.Name);
        }
        IEnumerable<PropertyInfo> serverInfos  = serverType.GetProperties(BIP);
        foreach (var serverInfo in serverInfos) { //遍历服务的info
            if (proName.Contains(serverInfo.Name)) {//ViewModel有这个属性
                 foreach (var thisInfo in viewModelInfos) { //遍历ViewModel的属性并找到这个属性
                    if (serverInfo.Name == thisInfo.Name) {
                        try {
                            serverInfo.SetValue(serverObject, thisInfo.GetValue(viewModelObject)); //设置serverobject的值为viewmodel
                            break;
                        }
                        catch (Exception e) {
                            InstanceLog.Erro(e.Message, e.StackTrace);
                            break;
                        }
                    }
                 }
            }
        }
    }
}
