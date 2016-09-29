using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
    public class RegulatoryDecision
    {
        
        public string LinkId { get; set; }        
        public string Drugname { get; set; }
        public string ContactName { get; set; }       
        public string ContactUrl { get; set; }        
        public string MedicalIngredient { get; set; }        
        public string TherapeuticArea { get; set; }       
        public string Purpose { get; set; }        
        public string ReasonDecision { get; set; }        
        public string Decision { get; set; }
        public string DecisionDescr { get; set; }
        public DateTime? DateDecision { get; set; }
        public string Manufacture { get; set; }
        public string PrescriptionStatus { get; set; }       
        public string TypeSubmission { get; set; }        
        public DateTime? DateFiled { get; set; }
        public int ControlNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Footnotes { get; set; }
        public string Din { get; set; }        
        public List<string> DinList { get; set; }
        public List<BullePoint> BulletList { get; set; }

    }
}