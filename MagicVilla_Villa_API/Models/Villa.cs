namespace MagicVilla_Villa_API.Models
{
    public class Villa
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}