namespace NapCatSprcit.MessagesService.SendMessagesClass
{
    public class ImageToBase64
    {
        public static string Method(string filePath)
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);
            return "base64://" + Convert.ToBase64String(imageBytes);
        }
    }
}
