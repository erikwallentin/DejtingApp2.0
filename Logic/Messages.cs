namespace Group11.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}