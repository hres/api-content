using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
    public class SafetyReview
    {
       
        public int template { get; set; }
        public string link_id { get; set; }
        public DateTime? review_date { get; set; }
        public string drug_name { get; set; }
        public string safety_issue { get; set; }
        
        public string issue { get; set; }
       
        public string background { get; set; }
        
        public string objective { get; set; }
        
        public string key_findings { get; set; }
       
        public int key_messages { get; set; }       

        public string overview { get; set; }
        
        public int use_canada { get; set; }
        
        public int findings { get; set; }
       
        public int conclusion { get; set; }
        
        public string additional { get; set; }
       
        public string full_review { get; set; }
       
        public int references { get; set; }
        
        public int footnotes { get; set; }
       
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }
        public List<BullePoint> key_message_list { get; set; }
        public List<BullePoint> footnotes_list { get; set; }
        public List<BullePoint> reference_list { get; set; }
        public List<BullePoint> use_canada_list { get; set; }
        public List<BullePoint> finding_list { get; set; }
        public List<BullePoint> conclusion_list { get; set; }
    }

    public class BullePoint 
    {
        public int field_id{ get; set; }
        public int order_no { get; set; }
        public string bullet { get; set; }    
    }
}