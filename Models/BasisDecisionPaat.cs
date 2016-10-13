using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
  
    public class BasisDecisionPaat
    {       

        public string link_id { get; set; }
        public DateTime? date_submit { get; set; }
        public DateTime? decision_start_date { get; set; }
        public DateTime? decision_end_date { get; set; }
        public int row_num { get; set; }
        public string act_contr_num { get; set; }
        public string submit_text { get; set; }
        public string paat_decision { get; set; }
        public string date_text { get; set; }
        public string summ_activity { get; set; }

    }

}
