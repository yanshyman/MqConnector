namespace MqConnector.Models
{
    public class MessageDTO
    {
        public string MessageID { get; set; }
        public string Content { get; set; }
        public string DataUTF8 { get; set; }
        public string DataBase64 { get; set; }
        public string ReplyToQueue { get; set; }
        public int MessageType { get; set; }
    }
}