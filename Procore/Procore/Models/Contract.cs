namespace Procore.Models
{
    public class Contract
    {
        public long? id { get; set; }
        public string accounting_method { get; set; }
        public object actual_completion_date { get; set; }
        public object approval_letter_date { get; set; }
        public string approved_change_orders { get; set; }
        public List<object> attachments { get; set; }
        public string billing_schedule_of_values_status { get; set; }
        public List<object> change_order_packages { get; set; }
        public object contract_date { get; set; }
        public object contract_estimated_completion_date { get; set; }
        public object contract_start_date { get; set; }
        public string created_at { get; set; }
        public int created_by_id { get; set; }
        public CurrencyConfiguration currency_configuration { get; set; }
        public Dictionary<string, object> custom_fields { get; set; }
        public object deleted_at { get; set; }
        public string description { get; set; }
        public string draft_change_orders_amount { get; set; }
        public bool enable_ssov { get; set; }
        public object exclusions { get; set; }
        public bool executed { get; set; }
        public object execution_date { get; set; }
        public string grand_total { get; set; }
        public bool has_change_order_packages { get; set; }
        public bool has_potential_change_orders { get; set; }
        public object inclusions { get; set; }
        public List<InvoiceContact> invoice_contacts { get; set; }
        public object issued_on_date { get; set; }
        public object letter_of_intent_date { get; set; }
        public List<LineItem> line_items { get; set; }
        public string number { get; set; }
        public object origin_code { get; set; }
        public object origin_data { get; set; }
        public object origin_id { get; set; }
        public List<object> payments_issued { get; set; }
        public string pending_change_orders { get; set; }
        public string pending_revised_contract { get; set; }
        public string percentage_paid { get; set; }
        public bool @private { get; set; }
        public Project project { get; set; }
        public string remaining_balance_outstanding { get; set; }
        public bool requisitions_are_enabled { get; set; }
        public string retainage_percent { get; set; }
        public object returned_date { get; set; }
        public string revised_contract { get; set; }
        public bool? show_line_items_to_non_admins { get; set; }
        public object signed_contract_received_date { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string total_draw_requests_amount { get; set; }
        public string total_payments { get; set; }
        public string total_requisitions_amount { get; set; }
        public string updated_at { get; set; }
        public Vendor vendor { get; set; }
    }

    public class CurrencyConfiguration
    {
        public object currency_iso_code { get; set; }
    }

    public class Parent
    {
        public long? id { get; set; }
    }

    public class LineItemType
    {
        public long? id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string base_type { get; set; }
        public object origin_id { get; set; }
    }

    public class Company
    {
        public long? id { get; set; }
        public string name { get; set; }
    }

    //public class CostCode
    //{
    //    public long id { get; set; }
    //    public string name { get; set; }
    //    public string full_code { get; set; }
    //    public object origin_id { get; set; }
    //    public object origin_data { get; set; }
    //    public object standard_cost_code_id { get; set; }
    //    public string biller { get; set; }
    //    public long biller_id { get; set; }
    //    public string biller_type { get; set; }
    //    public object biller_origin_id { get; set; }
    //    public bool budgeted { get; set; }
    //    public string code { get; set; }
    //    public Parent parent { get; set; }
    //    public string sortable_code { get; set; }
    //    public string created_at { get; set; }
    //    public object deleted_at { get; set; }
    //    public List<LineItemType> line_item_types { get; set; }
    //    public object position { get; set; }
    //    public string updated_at { get; set; }
    //}

    public class Holder
    {
        public long? id { get; set; }
        public string holder_type { get; set; }
    }

    public class LineItem
    {
        public long? id { get; set; }
        public string amount { get; set; }
        public Company company { get; set; }
        public CostCode cost_code { get; set; }
        public string created_at { get; set; }
        public CurrencyConfiguration currency_configuration { get; set; }
        public string description { get; set; }
        public string extended_amount { get; set; }
        public string extended_type { get; set; }
        public Holder holder { get; set; }
        public LineItemType line_item_type { get; set; }
        public object origin_code { get; set; }
        public object origin_data { get; set; }
        public object origin_id { get; set; }
        public int position { get; set; }
        public Project project { get; set; }
        public string quantity { get; set; }
        public long? tax_code_id { get; set; }
        public string total_amount { get; set; }
        public string unit_cost { get; set; }
        public object uom { get; set; }
        public string updated_at { get; set; }
        public WbsCode wbs_code { get; set; }
    }

    public class InvoiceContact
    {
        public long? id { get; set; }
        public object business_phone { get; set; }
        public object business_phone_extension { get; set; }
        public string email { get; set; }
        public string email_address { get; set; }
        public string fax_number { get; set; }
        public string job_title { get; set; }
        public long? login_information_id { get; set; }
        public object mobile_phone { get; set; }
        public string name { get; set; }
        public string vendor_name { get; set; }
    }

    public class Project
    {
        public long? id { get; set; }
        public string name { get; set; }
        public object origin_data { get; set; }
        public object origin_id { get; set; }
    }

    public class Vendor
    {
        public long id { get; set; }
        public string company { get; set; }
        public object origin_data { get; set; }
        public object origin_id { get; set; }
    }
    public class WbsCode
    {
        public string description { get; set; }
        public string flat_code { get; set; }
        public long? id { get; set; }
    }

}
