using System;
using System.Collections.Generic;

namespace Procore.Models
{
   
    public class CostCode
    {
        public long? Id { get; set; }
        public string name { get; set; }
        public string full_code { get; set; }
        public object origin_id { get; set; }
        public object origin_data { get; set; }
        public object standard_cost_code_id { get; set; }
        public string biller { get; set; }
        public long? biller_id { get; set; }
        public string biller_type { get; set; }
        public object biller_origin_id { get; set; }
        public bool budgeted { get; set; }
        public string code { get; set; }
        public Parent parent { get; set; }
        public string sortable_code { get; set; }
        public DateTime created_at { get; set; }
        public object deleted_at { get; set; }
        public List<object> line_item_types { get; set; }
        public object position { get; set; }
        public DateTime updated_at { get; set; }
    }


}
