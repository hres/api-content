using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
    public class BasisDecisionMedicalDevice
    {
        // From BasisDecision       
        public int template { get; set; }
        public string link_id { get; set; }        
        public string device_name { get; set; }
        public string application_num { get; set; }        
        public string recent_activity { get; set; }
        public DateTime? updated_date { get; set; }
        public string summary_basis_intro { get; set; }
        public string what_approved { get; set; }
        public string why_device_approved { get; set; }
        public string steps_approval_intro { get; set; }
        public string followup_measures { get; set; }
        public string post_licence_activity { get; set; }
        public string other_info { get; set; }
        public string scientific_rationale { get; set; }
        public string scientific_rationale2 { get; set; }
        public string scientific_rationale3 { get; set; }
        public DateTime? date_sbd_issued { get; set; }
        
        public string egalement { get; set; }
        public string manufacturer { get; set; }
        public string medical_device_group { get; set; }
        public string biological_material { get; set; }
        
        public string combination_product { get; set; }
        public string drug_material { get; set; }
        public string application_type_and_num { get; set; }
        public DateTime? date_licence_issued { get; set; }
        public string intended_use { get; set; }
        public string notice_of_decision { get; set; }
        public string sci_reg_basis_decision1 { get; set; }
        public string sci_reg_basis_decision2 { get; set; }
        public string sci_reg_basis_decision3 { get; set; }
        public string response_to_condition { get; set; }
        public string response_to_condition2 { get; set; }
        public string response_to_condition3 { get; set; }
        public string response_to_condition4 { get; set; }

        public string conclusion { get; set; }
        public string recommendation { get; set; }
        public string licence_number { get; set; }
        public bool is_md { get; set; }

        public List<PostLicensingActivity> plat_list { get; set; }
        public List<ApplicationMilestones> app_milestone_list { get; set; }

    }
public class PostLicensingActivity
{
    public string link_id { get; set; }
    public DateTime? date_submit { get; set; }  
    
    public int num_order { get; set; }
    public string app_type_num { get; set; }     

    public string decision_and_date { get; set; }
    public string summ_activity { get; set; }
}

public class ApplicationMilestones
    {

    public string link_id { get; set; }
    public int num_order { get; set; }
    public string application_milestone { get; set; }
    public DateTime? milestone_date { get; set; }
    public DateTime? milestone_date2 { get; set; }
    public string separator { get; set; }
}
}
