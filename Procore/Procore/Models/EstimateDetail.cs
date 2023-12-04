using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Procore.Models
{

    public class EstimateDetail
    {
        public long? id { get; set; }
        public int item_number { get; set; }
        public string item_type { get; set; }
        public long? cost_code_id { get; set; }
        public object description_of_work { get; set; }
        public string scheduled_value { get; set; }
        public string work_completed_from_previous_application { get; set; }
        public string work_completed_this_period { get; set; }
        public string materials_presently_stored { get; set; }
        public string total_completed_and_stored_to_date { get; set; }
        public string total_completed_and_stored_to_date_percent { get; set; }
        public string balance_to_finish { get; set; }
        public string work_completed_retainage_from_previous_application { get; set; }
        public string materials_stored_retainage_from_previous_application { get; set; }
        public string total_retainage_from_previous_application { get; set; }
        public string work_completed_retainage_percent_this_period { get; set; }
        public string materials_stored_retainage_percent_this_period { get; set; }
        public string work_completed_retainage_retained_this_period { get; set; }
        public string materials_stored_retainage_retained_this_period { get; set; }
        public string work_completed_retainage_released_this_period { get; set; }
        public string materials_stored_retainage_released_this_period { get; set; }
        public string work_completed_retainage_currently_retained { get; set; }
        public string materials_stored_retainage_currently_retained { get; set; }
        public string total_retainage_currently_retained { get; set; }
    }


}
