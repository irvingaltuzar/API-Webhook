namespace Procore.Models
{
    public class CompanyVendor
    {
        public long id { get; set; }
        public string abbreviated_name { get; set; }
        public string address { get; set; }
        public List<object> attachments { get; set; }
        public bool authorized_bidder { get; set; }
        public Bidding bidding { get; set; }
        public List<object> bidding_distribution { get; set; }
        public object business_id { get; set; }
        public string business_phone { get; set; }
        public object business_register { get; set; }
        public long? children_count { get; set; }
        public string city { get; set; }
        public string company { get; set; }
        public bool company_vendor { get; set; }
        public long? contact_count { get; set; }
        public string country_code { get; set; }
        public DateTime created_at { get; set; }
        public string email_address { get; set; }
        public string fax_number { get; set; }
        public bool is_active { get; set; }
        public string labor_union { get; set; }
        public string legal_name { get; set; }
        public string license_number { get; set; }
        public object logo { get; set; }
        public object mobile_phone { get; set; }
        public string name { get; set; }
        public bool non_union_prevailing_wage { get; set; }
        public string notes { get; set; }
        public object origin_code { get; set; }
        public object origin_data { get; set; }
        public object origin_id { get; set; }
        public object parent { get; set; }
        public bool prequalified { get; set; }
        public PrimaryContact primary_contact { get; set; }
        public List<long> project_ids { get; set; }
        public List<object> standard_cost_codes { get; set; }
        public string state_code { get; set; }
        public bool synced_to_erp { get; set; }
        public string trade_name { get; set; }
        public List<object> trades { get; set; }
        public bool union_member { get; set; }
        public DateTime updated_at { get; set; }
        public object vendor_group { get; set; }
        public string website { get; set; }
        public string zip { get; set; }
    }

    public class Bidding
    {
        public bool affirmative_action { get; set; }
        public bool african_american_business { get; set; }
        public bool asian_american_business { get; set; }
        public bool certified_business_enterprise { get; set; }
        public bool disadvantaged_business { get; set; }
        public bool eight_a_business { get; set; }
        public bool hispanic_business { get; set; }
        public bool historically_underutilized_business { get; set; }
        public bool minority_business_enterprise { get; set; }
        public bool native_american_business { get; set; }
        public bool sdvo_business { get; set; }
        public bool small_business { get; set; }
        public bool womens_business { get; set; }
    }

    public class PrimaryContact
    {
        public long? id { get; set; }
        public object business_phone { get; set; }
        public object business_phone_extension { get; set; }
        public DateTime created_at { get; set; }
        public string email_address { get; set; }
        public object fax_number { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public object mobile_phone { get; set; }
        public DateTime updated_at { get; set; }
    }
}