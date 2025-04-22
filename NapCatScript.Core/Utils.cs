namespace NapCatScript.Tool;

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
}
