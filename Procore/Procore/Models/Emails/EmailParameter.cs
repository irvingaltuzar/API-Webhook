namespace Procore.Models.Emails
{
    public class EmailParameter
    {
        public string? Name { get; set; }
        public List<string> Email { get; set; } 
        public string Message { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }
    }
}
