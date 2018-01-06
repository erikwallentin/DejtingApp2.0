namespace Group11.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ApplicationUser Receiver { get; set; }
        public ApplicationUser Sender { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}