using System.Reflection;

namespace NapCatScript.Core;

public static class Utils
{
    /// <summary>
    /// 给定一个源对象，给定一个目标对象，需要对象实例
    /// <para> 将源对象的属性值覆盖给目标对象，会覆盖传入对象的状态 </para>
    /// </summary>
    /// <returns>返回修改后的对象</returns>
    public static TarGet TypeMap<Source, TarGet>(Source source, TarGet tarGet)
        where TarGet : Source
    {
        System.Reflection.BindingFlags BIP = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
        var sourceProperts = typeof(Source).GetProperties(BIP);
        var targetProperts = typeof(TarGet).GetProperties(BIP);

        foreach (var sourceProp in sourceProperts) {
            var p = targetProperts.FirstOrDefault(p => p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);
            if (p != null) {
                object value = sourceProp.GetValue(source)!;
                p.SetValue(tarGet, value);
            }
        }
        return tarGet;
    }
    
    public static void TypeMap(Type thisType, Type serverType, object thisObject, object serverObject)
    {
        IEnumerable<PropertyInfo> Props1 = thisType.GetProperties();
        HashSet<string> proName = [];
        foreach (var propertyInfo in Props1) {
            proName.Add(propertyInfo.Name);
        }
        IEnumerable<PropertyInfo> Props2 = serverType.GetProperties();
        try {
            foreach (var thisInfo in Props2) {
                if (proName.Contains(thisInfo.Name)) {//httpserver有
                    foreach (var httpInfo in Props1) {
                        if (httpInfo.Name == thisInfo.Name) {
                            thisInfo.SetValue(thisObject, httpInfo.GetValue(serverObject));
                            break;
                        }
                    }
                }
            }
        } catch (Exception e) {
            Log.Erro(e.Message, e.StackTrace);
        }
    }
}
