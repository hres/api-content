using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
    public class BasisDecisionMilestone
    {
        public string link_id { get; set; }
        public int num_order { get; set; }
        public string milestone { get; set; }
        public DateTime? completed_date { get; set; }
        public DateTime? completed_date2 { get; set; }
        public string separator { get; set; }
    }
}