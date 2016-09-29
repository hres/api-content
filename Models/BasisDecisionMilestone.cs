using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
    public class BasisDecisionMilestone
    {
        
        public string LinkId { get; set; }
        public int NumOrder { get; set; }
        public string Milestone { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CompletedDate2 { get; set; }
        public string Separator { get; set; }
    }
}