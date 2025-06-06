﻿using SQLite;

namespace NapCatScript.Core.Model;
/// <summary>
/// 用于数据库使用的MesgInfo类
/// </summary>
public class SQLMesgInfo
{
    public readonly static string KeyName = nameof(SQLMesgKey);
    [PrimaryKey]
    public string SQLMesgKey { get; set; } = string.Empty;

    [SQLite.Column("Key")]
    public long Key { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    [SQLite.Column("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 群组ID
    /// </summary>
    [SQLite.Column("group_id")]
    public string GroupId { get; set; } = string.Empty;

    /// <summary>
    /// 用户名称
    /// </summary>
    [SQLite.Column("user_name")]
    public string UserName { get; set; } = "";

    /// <summary>
    /// 消息内容
    /// </summary>
    [SQLite.Column("message_content")]
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型
    /// </summary>
    [SQLite.Column("message_type")]
    public string MessageType { get; set; } = string.Empty;

    [SQLite.Column("time")]
    public double Time { get; set; }

    [SQLite.Column("message_id")]
    public long MessageId { get; set; }

    public string GetId()
    {
        //if (UserId != string.Empty) return UserId;
        //else return GroupId;
        if (MessageType == "group")
            return GroupId.ToString();
        else
            return UserId.ToString();
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
