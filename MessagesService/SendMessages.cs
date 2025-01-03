using System.Text;

namespace NapCatSprcit.MessagesService
{
    public partial class Messages
    {
        /// <summary>
        /// 发送Post请求 并返回回应消息
        /// <para></para>
        /// </summary>
        /// <param name="user_id"> 目标id </param>
        /// <param name="text"> 要发的内容 </param>
        /// <returns></returns>
        public async Task<string> SendPostMessagesAsync(string? httpUri, string msg, Encoding? enc ,string body = "application/json")
        {
            string HttpURI = httpUri ?? PublicProperty.HttpURI;
            Encoding encoding = enc ?? Encoding.UTF8;
            using HttpClient client = new HttpClient();

            //构建消息
            StringContent sc = new StringContent(msg, encoding, body);

            //发送并接收
            HttpResponseMessage HRM = await client.PostAsync(HttpURI, sc);

            return await HRM.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 发送Post请求 并返回回应消息
        /// </summary>
        /// <param name="httpUri"></param>
        /// <param name="sc"></param>
        /// <returns></returns>
        public async Task<string> SendPostMessagesAsync(string? httpUri, StringContent sc)
        {
            using HttpClient client = new HttpClient();
            string HttpURI = httpUri ?? PublicProperty.HttpURI;

            //发送并接收
            HttpResponseMessage HRM = await client.PostAsync(HttpURI, sc);

            return await HRM.Content.ReadAsStringAsync();
        }

    }
}
