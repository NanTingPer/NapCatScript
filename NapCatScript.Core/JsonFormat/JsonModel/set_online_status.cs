namespace NapCatScript.Core.JsonFormat.JsonModel;

public class set_online_status(set_online_status.OnlineType type) : RequestJson
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(type));

    private class Root
    {
        public Root(OnlineType type)
        {
            switch (type) {
                case OnlineType.在线:
                    Status = 10; ExtStatus = 0; BatteryStatus = 0;
                    break;
                case OnlineType.Q我吧:
                    Status = 60; ExtStatus = 0; BatteryStatus = 0;
                    break;
                case OnlineType.离开:
                    Status = 30; ExtStatus = 0; BatteryStatus = 0;
                    break;
                case OnlineType.忙碌:
                    Status = 50; ExtStatus = 0; BatteryStatus = 0;
                    break;
                case OnlineType.请勿打扰:
                    Status = 70; ExtStatus = 0; BatteryStatus = 0;
                    break;
                case OnlineType.隐身:
                    Status = 40; ExtStatus = 0; BatteryStatus = 0;
                    break;
                case OnlineType.听歌中:
                    Status = 10; ExtStatus = 1028; BatteryStatus = 0;
                    break;
                case OnlineType.春日限定:
                    Status = 10; ExtStatus = 2037; BatteryStatus = 0;
                    break;
                case OnlineType.一起元梦:
                    Status = 10; ExtStatus = 2025; BatteryStatus = 0;
                    break;
                case OnlineType.求星搭子:
                    Status = 10; ExtStatus = 2026; BatteryStatus = 0;
                    break;
                case OnlineType.被掏空:
                    Status = 10; ExtStatus = 2014; BatteryStatus = 0;
                    break;
                case OnlineType.今日天气:
                    Status = 10; ExtStatus = 1030; BatteryStatus = 0;
                    break;
                case OnlineType.我crash了:
                    Status = 10; ExtStatus = 2019; BatteryStatus = 0;
                    break;
                case OnlineType.爱你:
                    Status = 10; ExtStatus = 2006; BatteryStatus = 0;
                    break;
                case OnlineType.恋爱中:
                    Status = 10; ExtStatus = 1051; BatteryStatus = 0;
                    break;
                case OnlineType.好运锦鲤:
                    Status = 10; ExtStatus = 1071; BatteryStatus = 0;
                    break;
                case OnlineType.水逆退散:
                    Status = 10; ExtStatus = 1021; BatteryStatus = 0;
                    break;
                case OnlineType.嗨到飞起:
                    Status = 10; ExtStatus = 1056; BatteryStatus = 0;
                    break;
                case OnlineType.元气满满:
                    Status = 10; ExtStatus = 1058; BatteryStatus = 0;
                    break;
                case OnlineType.宝宝认证:
                    Status = 10; ExtStatus = 1070; BatteryStatus = 0;
                    break;
                case OnlineType.一言难尽:
                    Status = 10; ExtStatus = 1063; BatteryStatus = 0;
                    break;
                case OnlineType.难得糊涂:
                    Status = 10; ExtStatus = 2001; BatteryStatus = 0;
                    break;
                case OnlineType.emo中:
                    Status = 10; ExtStatus = 1401; BatteryStatus = 0;
                    break;
                case OnlineType.我太难了:
                    Status = 10; ExtStatus = 1062; BatteryStatus = 0;
                    break;
                case OnlineType.我想开了:
                    Status = 10; ExtStatus = 2013; BatteryStatus = 0;
                    break;
                case OnlineType.我没事:
                    Status = 10; ExtStatus = 1052; BatteryStatus = 0;
                    break;
                case OnlineType.想静静:
                    Status = 10; ExtStatus = 1061; BatteryStatus = 0;
                    break;
                case OnlineType.悠哉哉:
                    Status = 10; ExtStatus = 1059; BatteryStatus = 0;
                    break;
                case OnlineType.去旅行:
                    Status = 10; ExtStatus = 2015; BatteryStatus = 0;
                    break;
                case OnlineType.信号弱:
                    Status = 10; ExtStatus = 1011; BatteryStatus = 0;
                    break;
                case OnlineType.出去浪:
                    Status = 10; ExtStatus = 2003; BatteryStatus = 0;
                    break;
                case OnlineType.肝作业:
                    Status = 10; ExtStatus = 2012; BatteryStatus = 0;
                    break;
                case OnlineType.学习中:
                    Status = 10; ExtStatus = 1018; BatteryStatus = 0;
                    break;
                case OnlineType.搬砖中:
                    Status = 10; ExtStatus = 2023; BatteryStatus = 0;
                    break;
                case OnlineType.摸鱼中:
                    Status = 10; ExtStatus = 1300; BatteryStatus = 0;
                    break;
                case OnlineType.无聊中:
                    Status = 10; ExtStatus = 1060; BatteryStatus = 0;
                    break;
                case OnlineType.timi中:
                    Status = 10; ExtStatus = 1027; BatteryStatus = 0;
                    break;
                case OnlineType.睡觉中:
                    Status = 10; ExtStatus = 1016; BatteryStatus = 0;
                    break;
                case OnlineType.熬夜中:
                    Status = 10; ExtStatus = 1032; BatteryStatus = 0;
                    break;
                case OnlineType.追剧中:
                    Status = 10; ExtStatus = 1021; BatteryStatus = 0;
                    break;
                case OnlineType.我的电量:
                    Status = 10; ExtStatus = 1000 ; BatteryStatus = 0;
                    break;
            }
        }


        public int Status { get; set; }

        public int ExtStatus { get; set; }

        public int BatteryStatus { get; set; }
    }

    public enum OnlineType
    {
        在线,
        Q我吧,
        离开,
        忙碌,
        请勿打扰,
        隐身,
        听歌中,
        春日限定,
        一起元梦,
        求星搭子,
        被掏空,
        今日天气,
        我crash了,
        爱你,
        恋爱中,
        好运锦鲤,
        水逆退散,
        嗨到飞起,
        元气满满,
        宝宝认证,
        一言难尽,
        难得糊涂,
        emo中,
        我太难了,
        我想开了,
        我没事,
        想静静,
        悠哉哉,
        去旅行,
        信号弱,
        出去浪,
        肝作业,
        学习中,
        搬砖中,
        摸鱼中,
        无聊中,
        timi中,
        睡觉中,
        熬夜中,
        追剧中,
        我的电量,
    }
}
