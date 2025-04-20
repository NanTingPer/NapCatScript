
using TestPlugin.Models;

namespace TestPlugin;

public class VanillaWiki : PluginType
{
    public override void Init()
    {
        
    }

    public override async Task Run(MesgInfo mesg, string httpUri)
    {
        string nullTrim = mesg.MessageContent.Trim();
        if (!nullTrim.StartsWith('*'))
            return;

        string command = nullTrim.Split('*')[1]; //是指令 也是ItemName
        switch (command) {
            case "映射#":
                return;
            case "删除映射#":
                return;
        }

        string itemName = command;
        string itemPath = Path.Combine(Environment.CurrentDirectory, "Val", itemName + ".png");
        itemName = await WikiNameMapping<VanillaMapModel>.GetMap(itemName);

        await Task.Delay(0);
    }
}
