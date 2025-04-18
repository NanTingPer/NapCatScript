﻿namespace NapCatScript.Model;
/// <summary>
/// 数据信息 仅适用于接收消息
/// </summary>
public class MesgInfo
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 群组ID
    /// </summary>
    public string GroupId { get; set; } = string.Empty;

    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; } = "";

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型
    /// </summary>
    public string MessageType { get; set; } = string.Empty;

    /// <summary>
    /// 心跳
    /// </summary>
    public long lifeTime = 0;

    public string GetId()
    {
        //if (UserId != string.Empty) return UserId;
        //else return GroupId;
        if (MessageType == "group")
            return GroupId;
        else
            return UserId;
    }

    public override string ToString()
    {

        return
            "\r\n------------------------------------------------\r\n" +
            UserId + $": {UserName} :" +
            MessageContent + " ," +
            MessageType + "\r\n" +
            "------------------------------------------------\r\n";
    }
}