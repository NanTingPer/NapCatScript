namespace NapCatScript.Core.JsonFormat;

/// <summary>
/// 已有:
///     Json        Json卡片
///     Profile     设置个人消息
///     Image       图片消息
///     Video       视频消息
///     Record      语音消息
///     At          At消息
///     setonline   设置在线状态
///     ArkShare    获取群聊卡片
///     ArkShare    获取推荐好友
///     
/// No:
///     获取收藏表情 /fetch_custom_face
///     获取用户状态 /nc_get_user_status
///     获取小程序卡片 /get_mini_app_ark
///     获取私聊文件链接 /get_private_file_url
///     获取单向好友列表 /get_unidirectional_friend_list
///     设置自定义在线状态 /set_diy_online_status
/// </summary>
[Obsolete("", true)]
public abstract class BaseMsg
{
    public abstract string JsonText { get; set; }
    public abstract JsonElement JsonElement { get; set; }
    public abstract JsonDocument JsonDocument { get; set; }
    public abstract dynamic JsonObject { get; set; }
    public virtual string GetString()
    {
        return JsonText;
    }

    public override string ToString()
    {
        return JsonText;
    }
}


