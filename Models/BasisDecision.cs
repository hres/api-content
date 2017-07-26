using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
    public class BasisDecision
    {
        // From BasisDecision       
        public int template { get; set; }
        public string control_number { get; set; }
        public DateTime? date_issued { get; set; }
        public string link_id { get; set; }
        public string brand_name { get; set; }
        public string manufacturer { get; set; }
        public string med_ingredient { get; set; }
        public int bd_din_list { get; set; }
        public string sub_type_number { get; set; }
        public DateTime? date_submission { get; set; }
        public DateTime? date_authorization { get; set; }
        public string notice_decision { get; set; }
        public string sci_reg_decision { get; set; }
        public string quality_basis { get; set; }
        public string non_clin_basis { get; set; }
        public string non_clin_basis2 { get; set; }
        public string clin_basis { get; set; }
        public string clin_basis2 { get; set; }
        public string clin_basis3 { get; set; }
        public string benefit_risk { get; set; }
        public string radioisotope { get; set; }
        public string summary { get; set; }
        public string what_approved { get; set; }
        public string why_approved { get; set; }
        public string steps_approval { get; set; }
        public string assess_basis { get; set; }
        public string followup_measures { get; set; }
        public string post_auth { get; set; }
        public string other_info { get; set; }
        public string a_sci_reg_dcision { get; set; }
        public string science_rationale { get; set; }
        public string a_clin_basis { get; set; }
        public string a_clin_basis2 { get; set; }
        public string a_non_clin_basis { get; set; }
        public string a_non_clin_basis2 { get; set; }
        public string b_quality_basis { get; set; }
        public string contact { get; set; }
        public string paat_info { get; set; }
        public string summary_drug { get; set; }
        public string branch_info { get; set; }
        public string trademark { get; set; }        
        public string din { get; set; }
        public string licence_number{ get; set; }
        public bool is_md { get; set; }
        public List<string> din_list { get; set; }
        public List<PostAuthActivity> post_activity_list { get; set; }
        public List<DecisionMilestone> milestone_list { get; set; }
        public List<Tombstone> tombstone_list { get; set; }
    }
    public class PostAuthActivity
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

    public class DecisionMilestone
    {

        public string link_id { get; set; }
        public int num_order { get; set; }
        public string milestone { get; set; }
        public DateTime? completed_date { get; set; }
        public DateTime? completed_date2 { get; set; }
        public string separator { get; set; }
    }

    public class Tombstone
    {
        public string link_id { get; set; }
        public int num_order { get; set; }
        public string med_ingredient { get; set; }
        public string nonprop_name { get; set; }
        public string strength { get; set; }
        public string dosageform { get; set; }
        public string route_admin { get; set; }
        public string thera_class { get; set; }
        public string nonmed_ingredient { get; set; }       
    }
}