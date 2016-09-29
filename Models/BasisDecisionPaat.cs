using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
  
    public class BasisDecisionPaat
    {
        public string LinkedId { get; set; }
        public DateTime? DateSubmit { get; set; }
        public DateTime? DecisionStartDate { get; set; }
        public DateTime? DecisionEndDate { get; set; }
        public int RowNum { get; set; }
        public string ActContrNum { get; set; }
        public string SubmitText { get; set; }
        public string PaatDecision { get; set; }
        public string DateText { get; set; }
        public string SummActivity { get; set; }

    }

}
