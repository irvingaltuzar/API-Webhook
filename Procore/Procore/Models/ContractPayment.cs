namespace Procore.Models
{
    public class ContractPayment
    {
        public int id { get; set; }
        public long project_id { get; set; }
        public long contract_id { get; set; }
        public long company_id { get; set; }
        public string date { get; set; }
        public string invoice_number { get; set; }
        public string check_number { get; set; }
        public string invoice_date { get; set; }
        public int draw_request_number { get; set; }
        public string notes { get; set; }
        public string payment_number { get; set; }
        public string amount { get; set; }
    }
}
