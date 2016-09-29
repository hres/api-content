using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
    public class SafetyReview
    {
       
        public int Template { get; set; }
        public string LinkId { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string DrugName { get; set; }
        public string Safetyissue { get; set; }
        
        public string Issue { get; set; }
       
        public string Background { get; set; }
        
        public string Objective { get; set; }
        
        public string KeyFindings { get; set; }
       
        public int KeyMessages { get; set; }       

        public string Overview { get; set; }
        
        public int UseCanada { get; set; }
        
        public int Findings { get; set; }
       
        public int Conclusion { get; set; }
        
        public string Additional { get; set; }
       
        public string FullReview { get; set; }
       
        public int References { get; set; }
        
        public int Footnotes { get; set; }
       
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<BullePoint> KeyMessageList { get; set; }
        public List<BullePoint> FootnotesList { get; set; }
        public List<BullePoint> ReferenceList { get; set; }
        public List<BullePoint> UseCanadaList { get; set; }
        public List<BullePoint> FindingList { get; set; }
        public List<BullePoint> ConclusionList { get; set; }
    }

    public class BullePoint 
    {
        public int FieldId{ get; set; }
        public int OrderNo { get; set; }
        public string Bullet { get; set; }    
    }
}