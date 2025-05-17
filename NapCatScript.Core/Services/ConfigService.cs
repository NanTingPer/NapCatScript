using System.Text.RegularExpressions;

namespace NapCatScript.Core.Services;

public class ConfigService
{
    private SQLiteService sql { get; } = SQLiteService.SQLService;
    public static ConfigService Config { get; } = new ConfigService();
    private ConfigService(){}

    /// <summary>
    /// 获取配置项值，未找到会创建空配置。并返回string.Empty
    /// </summary>
    public async Task<string> GetConfig(string name)
    {
        ConfigModel? conf = await sql.Get<ConfigModel>(name);
        if (conf == null || conf == default) {
            await sql.Insert(new ConfigModel() { Name = name, Content = string.Empty });
            return string.Empty;
        }
        return conf.Content;
    }

    /// <summary>
    /// 设置配置项
    /// </summary>
    public async Task<bool> SetConfig(string name, string content)
    {
        ConfigModel? conf = new ConfigModel() { Name = name, Content = content };
        ConfigModel? con = await sql.Get<ConfigModel>(name);
        if(con == null || con == default) {
            try {
                await sql.Insert(conf);
                return true;
            } catch (Exception e) {
                InstanceLog.Erro(e.Message, e.StackTrace);
                return false;
            }
        }

        try {
            await sql.Update(conf);
            return true;
        }catch (Exception e) {
            InstanceLog.Erro(e.Message, e.StackTrace);
            return false;
        }
    }

    /// <summary>
    /// 设置配置值
    /// </summary>
    public Task<bool> SetConfig(ConfigModel config)
    {
        return SetConfig(config.Name, config.Content);
    }

    /// <summary>
    /// 给定行内容 解析为配置。会覆盖数据库，返回解析后的配置值
    /// <para> 无效配置返回string.Empty 并不写入数据库 </para>
    /// </summary>
    public async Task<string> ParConfig(string line)
    {
        string notBarkString = Regex.Replace(line, @"\s", "");
        string[] conf = notBarkString.Split(":");
        if(conf.Length != 2) {
            InstanceLog.Waring(line, "无效配置");
            return string.Empty;
        }
        ConfigModel confm = new ConfigModel() { Name = conf[0], Content = conf[1] };
        string cont = await GetConfig(confm.Name);
        if (cont != string.Empty)
            await sql.Update(confm);

        return confm.Content;
    }
}
