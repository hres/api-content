using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
    public class RegulatoryDecision
    {
        public string link_id { get; set; }        
        public string drug_name { get; set; }
        public string contact_name { get; set; }       
        public string contact_url { get; set; }        
        public string medical_ingredient { get; set; }        
        public string therapeutic_area { get; set; }       
        public string purpose { get; set; }        
        public string reason_decision { get; set; }        
        public string decision { get; set; }
        public string decision_descr { get; set; }
        public DateTime? date_decision { get; set; }
        public string manufacturer { get; set; }
        public string prescription_status { get; set; }       
        public string type_submission { get; set; }        
        public DateTime? date_filed { get; set; }
        public int control_number { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }
        public int footnotes { get; set; }
        public string din { get; set; }
        public bool is_md { get; set; }
        public List<string> din_list { get; set; }
        public List<BullePoint> bullet_list { get; set; }
    }

    public class RegulatoryDecisionMedicalDevice
    {
        public string link_id { get; set; }
        public string drug_name { get; set; }
        public string contact_name { get; set; }
        public string contact_url { get; set; }
        public string decision { get; set; }
        public string decision_descr { get; set; }
        public DateTime? date_decision { get; set; }
        public string type_application { get; set; }
        public DateTime? date_filed { get; set; }
        public int application_number { get; set; }
        public bool is_md { get; set; }
        public string device_class { get; set; }
        public string what_app_for { get; set; }
        public string info_reviewed { get; set; }
        public string licence_num_issued { get; set; }
        public string manufacturer { get; set; }
        public string medical_ingredient { get; set; }
    }
}