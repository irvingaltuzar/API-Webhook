namespace Procore.Models
{
    public class WebHookProcore
    {

        public string api_version { get; set; } = null!;
        public long? company_id { get; set; } = null!;

        public string event_type { get; set; } = null!;

        public long? id { get; set; }

        public object metadata { get; set; } = null!;   

        public long? project_id { get; set; }
        public long resource_id { get; set; }
        public string resource_name { get; set; } = null!;
        public DateTime? timestamp { get; set; }
        public string ulid { get; set; } = null!;   
        public long? user_id { get; set; }
        
    }
}
