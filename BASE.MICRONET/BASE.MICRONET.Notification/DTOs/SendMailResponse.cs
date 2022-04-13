namespace BASE.MICRONET.Notification.DTOs
{
    public class SendMailResponse
    {
        public int SendMailId { get; set; }
        public string SendDate { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Address { get; set; }
        public int AccountId { get; set; }
    }
}
