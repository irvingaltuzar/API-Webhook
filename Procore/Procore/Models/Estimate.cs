using System;


namespace Procore.Models
{

    public class Estimate
    {
        public string[] attachments { get; set; }
        public DateTime billing_date { get; set; }
        public string comment { get; set; }
        public long? commitment_id { get; set; }
        public string commitment_type { get; set; }
        public string contract_name { get; set; }
        public DateTime created_at { get; set; }
        public CreatedBy created_by { get; set; }
        public CurrencyConfiguration currency_configuration { get; set; }
        public Dictionary<string, object> custom_fields { get; set; }
        public bool deletable { get; set; }
        public object electronic_signature_id { get; set; }
        public string erp_status { get; set; }
        public bool final { get; set; }
        public long? id { get; set; }
        public string invoice_number { get; set; }
        public int number { get; set; }
        public object origin_data { get; set; }
        public object origin_id { get; set; }
        public DateTime payment_date { get; set; }
        public PaymentSummary payment_summary { get; set; }
        public string percent_complete { get; set; }
        public long? period_id { get; set; }
        public long? previous_requisition_id { get; set; }
        public long? project_id { get; set; }
        public DateTime requisition_end { get; set; }
        public DateTime requisition_start { get; set; }
        public string status { get; set; }
        public DateTime submitted_at { get; set; }
        public Summary summary { get; set; }
        public decimal total_claimed_amount { get; set; }
        public DateTime updated_at { get; set; }
        public long? vendor_id { get; set; }
        public string vendor_name { get; set; }
    }

    public class CreatedBy
    {
        public long? id { get; set; }
        public string company_name { get; set; }
        public string login { get; set; }
        public string name { get; set; }
    }
    public class PaymentSummary
    {
        public bool invoice_paid_in_full { get; set; }
        public string invoiced_amount_due { get; set; }
        public ProcorePay procore_pay { get; set; }
        public Manual manual { get; set; }
    }

    public class ProcorePay
    {
        public decimal amount { get; set; }
    }

    public class Manual
    {
        public decimal amount { get; set; }
    }

    public class Summary
    {
        public string balance_to_finish_including_retainage { get; set; }
        public string completed_work_retainage_amount { get; set; }
        public string completed_work_retainage_percent { get; set; }
        public string contract_sum_to_date { get; set; }
        public string current_payment_due { get; set; }
        public string formatted_period { get; set; }
        public string less_previous_certificates_for_payment { get; set; }
        public string negative_change_order_item_total { get; set; }
        public string negative_new_change_order_item_total { get; set; }
        public string negative_previous_change_order_item_total { get; set; }
        public string net_change_by_change_orders { get; set; }
        public string original_contract_sum { get; set; }
        public string positive_change_order_item_total { get; set; }
        public string positive_new_change_order_item_total { get; set; }
        public string positive_previous_change_order_item_total { get; set; }
        public string stored_materials_retainage_amount { get; set; }
        public string stored_materials_retainage_percent { get; set; }
        public string tax_applicable_to_this_payment { get; set; }
        public string total_completed_and_stored_to_date { get; set; }
        public string total_earned_less_retainage { get; set; }
        public string total_retainage { get; set; }
    }

}
