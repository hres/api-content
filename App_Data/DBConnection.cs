using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using regContentWebApi.Models;
using Npgsql;
using System.Text;
using System.Linq;

namespace regContentWebApi
{

    public class DBConnection
    {

        private string _lang;
        public string Lang
        {
            get { return this._lang; }
            set { this._lang = value; }
        }

        public DBConnection(string lang)
        {
            this._lang = lang;
        }

        private string RCDBConnection
        {
            get { return ConfigurationManager.ConnectionStrings["regContent"].ToString(); }
        }


        public List<BasisDecision> GetAllBasisDecision()
        {
            var items = new List<BasisDecision>();


            string commandText = string.Empty;
            commandText = "SELECT a.link_id, a.brandname, a.manufacturer, a.date_issued, a.control_num, a.template ";
            commandText += "FROM SBD as a, bd_tombstone as b WHERE a.link_id = b.link_id AND ";
            

            if (this.Lang.Equals("fr"))
            {
                commandText += " upper(b.language)='FRENCH';";
            }
            else
            {
                commandText += " upper(b.language)='ENGLISH';";
            }
           

            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                try
                   {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new BasisDecision();
                                    item.control_number = dr["control_num"] == DBNull.Value ? string.Empty : dr["control_num"].ToString().Trim();
                                    item.date_issued = dr["date_issued"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_issued"]);
                                    item.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.brand_name = dr["brandname"] == DBNull.Value ? string.Empty : dr["brandname"].ToString().Trim();
                                    item.manufacturer = dr["manufacturer"] == DBNull.Value ? string.Empty : dr["manufacturer"].ToString().Trim();                                    
                                    item.med_ingredient = "";
                                    item.template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.is_md = false;
                                    item.licence_number = "";
                                    items.Add(item);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetAllBasisDecision()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
            }
            return items;
        }


        public List<BasisDecision> GetAllBasisDecisionMedicalDevice()
        {
            var items = new List<BasisDecision>();

            string commandText = string.Empty;
            commandText = "SELECT a.link_id as link_id, a.device_name as device_name, a.manufacturer as manufacturer, a.updated_date as updated_date, ";
            commandText += "a.application_num as application_num, a.template as template ,b.licence_num as licence_num FROM sbd_devices as a ";
            commandText += " Left Join sbd_med_licence as b ON a.link_id = b.link_id WHERE ";            
            if (this.Lang.Equals("fr"))
            {
                
                commandText += " Upper(a.language)='FRENCH' AND Upper(b.language)='FRENCH'";
            }
            else
            {
                commandText += " Upper(a.language)='ENGLISH' AND Upper(b.language)='ENGLISH'";
            }
            
            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                    try
                    {
                        con.Open();
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new BasisDecision();
                                    item.control_number = dr["application_num"] == DBNull.Value ? string.Empty : dr["application_num"].ToString().Trim();
                                    item.date_issued = dr["updated_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["updated_date"]);
                                    item.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.brand_name = dr["device_name"] == DBNull.Value ? string.Empty : dr["device_name"].ToString().Trim();
                                    item.manufacturer = dr["manufacturer"] == DBNull.Value ? string.Empty : dr["manufacturer"].ToString().Trim();
                                    item.med_ingredient = "N/A";
                                    item.template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.is_md = true;
                                    item.licence_number = dr["licence_num"] == DBNull.Value ? string.Empty : dr["licence_num"].ToString().Trim();
                                    items.Add(item);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetAllBasisDecision()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
            }
            return items;
        }

        public BasisDecision GetBasisDecisionById(string id)
        {
            
            var returnItem = new BasisDecision();
            var items = new List<BasisDecision>();

            string commandText = string.Empty;
            commandText = "SELECT a.link_id, a.template, a.date_submission, a.date_authorization, a.brandname, a.manufacturer, a.date_issued, a.control_num, a.bd_din_list, ";
                       // + " b.nonprop_name as nonprop_name, b.strength as strength,  b.dosageform as dosageform, b.route_admin as route_admin, "
                       // + " b.thera_class as thera_class, b.med_ingredient as med_ingredient, b.nonmed_ingredient as nonmed_ingredient,";

            if (this.Lang.Equals("fr"))
            {
                commandText +=" a.sub_type_num_fr as sub_type_num, a.notice_decision_fr as notice_decision,"
                            + " a.sci_reg_decision_fr as sci_reg_decision, a.quality_basis_fr as quality_basis, a.nonclin_basis_fr as nonclin_basis,"
                            + " a.nonclin_basis2_fr as nonclin_basis2, a.clin_basis_fr as clin_basis, a.clin_basis2_fr as clin_basis2, a.clin_basis3_fr as clin_basis3,"
                            + " a.benefit_risk_fr as benefit_risk, a.radioisotope_fr as radioisotope, a.summary_fr as summary,"
                            + " a.what_approved_fr as what_approved, a.why_approved_fr as why_approved, a.steps_approval_fr as steps_approval, a.assess_basis_fr as assess_basis, "
                            + " a.followup_measures_fr as followup_measures, "
                            + " a.post_auth_fr as post_auth, other_info_fr as other_info, a.a_sci_reg_decision_fr as a_sci_reg_decision,"
                            + " a.science_rationale_fr as science_rationale, a.a_clin_basis_fr as a_clin_basis, a.a_clin_basis2_fr as a_clin_basis2,"
                            + " a.a_non_clin_basis_fr as a_non_clin_basis, a.a_non_clin_basis2_fr as a_non_clin_basis2, a.b_quality_basis_fr as b_quality_basis, a.contact_fr as contact"                            
                            + " FROM SBD as a WHERE a.link_ID = @link_id;";


            }
            else
            {
                commandText += "a.sub_type_num_en as sub_type_num, a.notice_decision_en as notice_decision,"
                            + " a.sci_reg_decision_en as sci_reg_decision, a.quality_basis_en as quality_basis, a.nonclin_basis_en as nonclin_basis,"
                            + " a.nonclin_basis2_en as nonclin_basis2, a.clin_basis_en as clin_basis, a.clin_basis2_en as clin_basis2, a.clin_basis3_en as clin_basis3,"
                            + " a.benefit_risk_en as benefit_risk, a.radioisotope_en as radioisotope, a.summary_en as summary,"
                            + " a.what_approved_en as what_approved, a.why_approved_en as why_approved, a.steps_approval_en as steps_approval, a.assess_basis_en as assess_basis, "
                            + " a.followup_measures_en as followup_measures, "
                            + " a.post_auth_en as post_auth, other_info_en as other_info, a.a_sci_reg_decision_en as a_sci_reg_decision,"
                            + " a.science_rationale_en as science_rationale, a.a_clin_basis_en as a_clin_basis, a.a_clin_basis2_en as a_clin_basis2,"
                            + " a.a_non_clin_basis_en as a_non_clin_basis, a.a_non_clin_basis2_en as a_non_clin_basis2, a.b_quality_basis_en as b_quality_basis, a.contact_en as contact"                            
                            + " FROM SBD as a WHERE a.link_ID = @link_id;";
            }
                
                 


            using ( NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                    {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new BasisDecision();
                                    item.template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.control_number = dr["control_num"] == DBNull.Value ? string.Empty : dr["control_num"].ToString().Trim();
                                    item.date_issued = dr["date_issued"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_issued"]);
                                    item.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.brand_name = dr["brandname"] == DBNull.Value ? string.Empty : dr["brandname"].ToString().Trim();
                                    item.manufacturer = dr["manufacturer"] == DBNull.Value ? string.Empty : dr["manufacturer"].ToString().Trim();
                                    //item.med_ingredient = dr["med_ingredient"] == DBNull.Value ? string.Empty : dr["med_ingredient"].ToString().Trim();
                                    //item.non_prop_name = dr["nonprop_name"] == DBNull.Value ? string.Empty : dr["nonprop_name"].ToString().Trim();
                                    //item.strength = dr["strength"] == DBNull.Value ? string.Empty : dr["strength"].ToString().Trim();
                                    //item.dosage_form = dr["dosageform"] == DBNull.Value ? string.Empty : dr["dosageform"].ToString().Trim();
                                    //item.route_admin = dr["route_admin"] == DBNull.Value ? string.Empty : dr["route_admin"].ToString().Trim();
                                    item.bd_din_list = dr["bd_din_list"] == DBNull.Value ? 0 : Convert.ToInt32(dr["bd_din_list"]);
                                    //item.thera_class = dr["thera_class"] == DBNull.Value ? string.Empty : dr["thera_class"].ToString().Trim();
                                    //item.non_med_ingredient = dr["nonmed_ingredient"] == DBNull.Value ? string.Empty : dr["nonmed_ingredient"].ToString().Trim();
                                    item.sub_type_number = dr["sub_type_num"] == DBNull.Value ? string.Empty : dr["sub_type_num"].ToString().Trim();
                                    item.date_submission = dr["date_submission"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_submission"]);
                                    item.date_authorization = dr["date_authorization"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_authorization"]);
                                    item.notice_decision = dr["notice_decision"] == DBNull.Value ? string.Empty : dr["notice_decision"].ToString().Trim();
                                    item.sci_reg_decision = dr["sci_reg_decision"] == DBNull.Value ? string.Empty : dr["sci_reg_decision"].ToString().Trim();
                                    item.quality_basis = dr["quality_basis"] == DBNull.Value ? string.Empty : dr["quality_basis"].ToString().Trim();
                                    item.non_clin_basis = dr["nonclin_basis"] == DBNull.Value ? string.Empty : dr["nonclin_basis"].ToString().Trim();
                                    item.non_clin_basis2 = dr["nonclin_basis2"] == DBNull.Value ? string.Empty : dr["nonclin_basis2"].ToString().Trim();
                                    item.clin_basis = dr["clin_basis"] == DBNull.Value ? string.Empty : dr["clin_basis"].ToString().Trim();
                                    item.clin_basis2 = dr["clin_basis2"] == DBNull.Value ? string.Empty : dr["clin_basis2"].ToString().Trim();
                                    item.clin_basis3 = dr["clin_basis3"] == DBNull.Value ? string.Empty : dr["clin_basis3"].ToString().Trim();
                                    item.benefit_risk = dr["benefit_risk"] == DBNull.Value ? string.Empty : dr["benefit_risk"].ToString().Trim();
                                    item.radioisotope = dr["radioisotope"] == DBNull.Value ? string.Empty : dr["radioisotope"].ToString().Trim();
                                    item.summary = dr["summary"] == DBNull.Value ? string.Empty : dr["summary"].ToString().Trim();
                                    item.what_approved = dr["what_approved"] == DBNull.Value ? string.Empty : dr["what_approved"].ToString().Trim();
                                    item.why_approved = dr["why_approved"] == DBNull.Value ? string.Empty : dr["why_approved"].ToString().Trim();
                                    item.steps_approval = dr["steps_approval"] == DBNull.Value ? string.Empty : dr["steps_approval"].ToString().Trim();
                                    item.assess_basis = dr["assess_basis"] == DBNull.Value ? string.Empty : dr["assess_basis"].ToString().Trim();
                                    item.followup_measures = dr["followup_measures"] == DBNull.Value ? string.Empty : dr["followup_measures"].ToString().Trim();
                                    item.post_auth = dr["post_auth"] == DBNull.Value ? string.Empty : dr["post_auth"].ToString().Trim();
                                    item.other_info = dr["other_info"] == DBNull.Value ? string.Empty : dr["other_info"].ToString().Trim();
                                    item.a_sci_reg_dcision = dr["a_sci_reg_decision"] == DBNull.Value ? string.Empty : dr["a_sci_reg_decision"].ToString().Trim();
                                    item.science_rationale = dr["science_rationale"] == DBNull.Value ? string.Empty : dr["science_rationale"].ToString().Trim();
                                    item.a_clin_basis = dr["a_clin_basis"] == DBNull.Value ? string.Empty : dr["a_clin_basis"].ToString().Trim();
                                    item.a_clin_basis2 = dr["a_clin_basis2"] == DBNull.Value ? string.Empty : dr["a_clin_basis2"].ToString().Trim();
                                    item.a_non_clin_basis = dr["a_non_clin_basis"] == DBNull.Value ? string.Empty : dr["a_non_clin_basis"].ToString().Trim();
                                    item.a_non_clin_basis2 = dr["a_non_clin_basis2"] == DBNull.Value ? string.Empty : dr["a_non_clin_basis2"].ToString().Trim();
                                    item.b_quality_basis = dr["b_quality_basis"] == DBNull.Value ? string.Empty : dr["b_quality_basis"].ToString().Trim();
                                    item.contact = dr["contact"] == DBNull.Value ? string.Empty : dr["contact"].ToString().Trim();
                                    items.Add(item);
                                }

                                if (items != null && items.Count > 0)
                                {
                                    returnItem = items.FirstOrDefault();
                                    returnItem.din_list = new List<string>();
                                    returnItem.tombstone_list = new List<Tombstone>();

                                    foreach (var temp in items)
                                    {
                                        if (!string.IsNullOrWhiteSpace(temp.din))
                                            returnItem.din_list.Add(temp.din);
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetBasisDecisionById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }            
            return returnItem;
        }


        public BasisDecisionMedicalDevice GetBasisDecisionMedicalDeviceById(string id)
        {
            var returnItem = new BasisDecisionMedicalDevice();
            var items = new List<BasisDecisionMedicalDevice>();

            string commandText = string.Empty;
            
            commandText = "SELECT a.link_id, a.template, a.device_name, a.application_num, a.recent_activity, a.updated_date, a.summary_basis_intro,"
                        + "a.what_approved, a.why_device_approved, a.steps_approval_intro, a.followup_measures, a.post_licence_activity, a.other_info,"
                        + "a.scientific_rationale, a.scientific_rationale2, a.scientific_rationale3, a.date_sbd_issued, a.egalement, a.manufacturer, "
                        + "a.medical_device_group, a.biological_material, a.combination_product, a. drug_material, a.application_type_and_num, a.date_licence_issued,"
                        + "a.intended_use, a.notice_of_decision, a.sci_reg_basis_decision1, a.sci_reg_basis_decision2, a.sci_reg_basis_decision3, a.response_to_condition,"
                        + "a.conclusion, a.recommendation, b.licence_num FROM sbd_devices as a "
                        + "LEFT OUTER JOIN sbd_med_licence as b ON a.link_id = b.link_id WHERE a.link_ID = @link_id AND";
            

            if (this.Lang.Equals("fr"))
            {
                commandText += " upper(a.language)='FRENCH' AND upper(b.language)='FRENCH';";

            }
            else
            {
                commandText += " upper(a.language)='ENGLISH' AND upper(b.language)='ENGLISH';";
            }            
            
            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                    {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new BasisDecisionMedicalDevice();
                                    item.template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.device_name = dr["device_name"] == DBNull.Value ? string.Empty : dr["device_name"].ToString().Trim();
                                    item.application_num = dr["application_num"] == DBNull.Value ? string.Empty : dr["application_num"].ToString().Trim();
                                    item.recent_activity = dr["recent_activity"] == DBNull.Value ? string.Empty : dr["recent_activity"].ToString().Trim();
                                    item.updated_date = dr["updated_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["updated_date"]);
                                    item.summary_basis_intro = dr["summary_basis_intro"] == DBNull.Value ? string.Empty : dr["summary_basis_intro"].ToString().Trim();
                                    item.what_approved = dr["what_approved"] == DBNull.Value ? string.Empty : dr["what_approved"].ToString().Trim();
                                    item.why_device_approved = dr["why_device_approved"] == DBNull.Value ? string.Empty : dr["why_device_approved"].ToString().Trim();
                                    item.steps_approval_intro = dr["steps_approval_intro"] == DBNull.Value ? string.Empty : dr["steps_approval_intro"].ToString().Trim();
                                    item.followup_measures = dr["followup_measures"] == DBNull.Value ? string.Empty : dr["followup_measures"].ToString().Trim();
                                    item.post_licence_activity = dr["post_licence_activity"] == DBNull.Value ? string.Empty : dr["post_licence_activity"].ToString().Trim();
                                    item.other_info = dr["other_info"] == DBNull.Value ? string.Empty : dr["other_info"].ToString().Trim();
                                    item.scientific_rationale = dr["scientific_rationale"] == DBNull.Value ? string.Empty : dr["scientific_rationale"].ToString().Trim();
                                    item.scientific_rationale2 = dr["scientific_rationale2"] == DBNull.Value ? string.Empty : dr["scientific_rationale2"].ToString().Trim();
                                    item.scientific_rationale3 = dr["scientific_rationale3"] == DBNull.Value ? string.Empty : dr["scientific_rationale3"].ToString().Trim();
                                    item.date_sbd_issued = dr["date_sbd_issued"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_sbd_issued"]);
                                    item.egalement = dr["egalement"] == DBNull.Value ? string.Empty : dr["egalement"].ToString().Trim();
                                    item.manufacturer = dr["manufacturer"] == DBNull.Value ? string.Empty : dr["manufacturer"].ToString().Trim();
                                    item.medical_device_group = dr["medical_device_group"] == DBNull.Value ? string.Empty : dr["medical_device_group"].ToString().Trim();
                                    item.biological_material = dr["biological_material"] == DBNull.Value ? string.Empty : dr["biological_material"].ToString().Trim();
                                    item.combination_product = dr["combination_product"] == DBNull.Value ? string.Empty : dr["combination_product"].ToString().Trim();
                                    item.drug_material = dr["drug_material"] == DBNull.Value ? string.Empty : dr["drug_material"].ToString().Trim();
                                    item.application_type_and_num = dr["application_type_and_num"] == DBNull.Value ? string.Empty : dr["application_type_and_num"].ToString().Trim();
                                    item.date_licence_issued = dr["date_licence_issued"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_licence_issued"]);
                                    item.intended_use = dr["intended_use"] == DBNull.Value ? string.Empty : dr["intended_use"].ToString().Trim();
                                    item.notice_of_decision = dr["notice_of_decision"] == DBNull.Value ? string.Empty : dr["notice_of_decision"].ToString().Trim();
                                    item.sci_reg_basis_decision1 = dr["sci_reg_basis_decision1"] == DBNull.Value ? string.Empty : dr["sci_reg_basis_decision1"].ToString().Trim();
                                    item.sci_reg_basis_decision2 = dr["sci_reg_basis_decision2"] == DBNull.Value ? string.Empty : dr["sci_reg_basis_decision2"].ToString().Trim();
                                    item.sci_reg_basis_decision3 = dr["sci_reg_basis_decision3"] == DBNull.Value ? string.Empty : dr["sci_reg_basis_decision3"].ToString().Trim();
                                    item.response_to_condition = dr["response_to_condition"] == DBNull.Value ? string.Empty : dr["response_to_condition"].ToString().Trim();
                                    item.conclusion = dr["conclusion"] == DBNull.Value ? string.Empty : dr["conclusion"].ToString().Trim();
                                    item.recommendation = dr["recommendation"] == DBNull.Value ? string.Empty : dr["recommendation"].ToString().Trim();
                                    item.licence_number = dr["licence_num"] == DBNull.Value ? string.Empty : dr["licence_num"].ToString().Trim();
                                    item.is_md = true;
                                    items.Add(item);
                                }


                                if (items != null && items.Count > 0)
                                {
                                    returnItem = items.FirstOrDefault();

                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetBasisDecisionById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            return returnItem;
        }

        public List<SafetyReview> GetAllSafetyReview()
        {
            var items = new List<SafetyReview>();
            
            string commandText = string.Empty;
            commandText = "SELECT link_id, template, drug_name, created_date, modified_date,";
            if (this.Lang.Equals("fr"))
            {
                commandText += " safteyissue_fr as safteyissue";
            }
            else
            {
                commandText += " safteyissue_en as safteyissue";
            }
            commandText += " FROM SSR";
            
            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    try
                    {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new SafetyReview();
                                    item.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.drug_name = dr["drug_name"] == DBNull.Value ? string.Empty : dr["drug_name"].ToString().Trim();
                                    item.safety_issue = dr["safteyissue"] == DBNull.Value ? string.Empty : dr["safteyissue"].ToString().Trim();
                                    item.created_date = dr["created_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["created_date"]); ;
                                    item.modified_date = dr["modified_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["modified_date"]);
                                    items.Add(item);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetAllSafetyReview()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            return items;
        }

        public List<BullePoint> GetSafetyReviewBulletListById(string id)
        {
            var items = new List<BullePoint>();

            string commandText = string.Empty;
            commandText = "SELECT num_order as orderNo, ";
            if (this.Lang.Equals("fr"))
            {
                commandText += "field_id_fr as fieldID, bullet_value_fr as bullet";
            }
            else
            {
                commandText += "field_id_en as fieldID, bullet_value_en as bullet";
            }
            commandText += " FROM ssr_bullet_points WHERE link_ID = @link_id";

            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                    {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var bullet = new BullePoint();
                                    bullet.field_id = dr["fieldID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["fieldID"].ToString().Trim());
                                    bullet.order_no = dr["orderNo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["orderNo"].ToString().Trim());
                                    bullet.bullet = dr["bullet"] == DBNull.Value ? string.Empty : dr["bullet"].ToString().Trim();
                                    items.Add(bullet);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetSafetyReviewBulletListById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            return items;
        }

        public List<PostAuthActivity> GetPostAuthActivityListById(string id)
        {
            var items = new List<PostAuthActivity>();

            string commandText = string.Empty;
            commandText = "SELECT link_id, row_num, act_contr_num, date_submit, date_submit_text, paat_decision, paat_decision_start_date, paat_date_text, paat_decision_end_date, summ_activ FROM paat WHERE link_ID = @link_id";

            if (this.Lang.Equals("fr"))
            {
                commandText += " AND language='french';";
            }
            else
            {
                commandText += " AND language='english';";
            }

            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                        {
    
                            using (NpgsqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        var paat = new PostAuthActivity();
                                        paat.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                        paat.row_num = dr["row_num"] == DBNull.Value ? 0 : Convert.ToInt32(dr["row_num"].ToString().Trim());
                                        paat.act_contr_num = dr["act_contr_num"] == DBNull.Value ? string.Empty : dr["act_contr_num"].ToString().Trim();
                                        paat.date_submit = dr["date_submit"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_submit"]);
                                        paat.submit_text = dr["date_submit_text"] == DBNull.Value ? string.Empty : dr["date_submit_text"].ToString().Trim();
                                        paat.paat_decision = dr["paat_decision"] == DBNull.Value ? string.Empty : dr["paat_decision"].ToString().Trim();
                                        paat.decision_start_date = dr["paat_decision_start_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["paat_decision_start_date"]);
                                        paat.date_text = dr["paat_date_text"] == DBNull.Value ? string.Empty : dr["paat_date_text"].ToString().Trim();
                                        paat.decision_end_date = dr["paat_decision_end_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["paat_decision_end_date"]);
                                        paat.summ_activity = dr["summ_activ"] == DBNull.Value ? string.Empty : dr["summ_activ"].ToString().Trim();
                                        items.Add(paat);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMessages = string.Format("DbConnection.cs - GetPostAuthActivityListById()");
                            ExceptionHelper.LogException(ex, errorMessages);
                            Console.WriteLine(errorMessages);
                        }
                        finally
                        {
                            if (con.State == ConnectionState.Open)
                                con.Close();
                        }
                    }
            }
            return items;
        }


        public List<PostLicensingActivity> GetPostLicensingActivityListById(string id)
        {
            var items = new List<PostLicensingActivity>();

            string commandText = string.Empty;
            commandText = "SELECT link_id, num_order, app_type_num, date_submitted, decision_and_date,summ_activities FROM plat WHERE link_ID = @link_id AND ";
            if (this.Lang.Equals("fr"))
            {
                commandText += " upper(language)='FRENCH';";
            }
            else
            {
                commandText += " upper(language)='ENGLISH';";
            }
            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                    {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var plat = new PostLicensingActivity();
                                    plat.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    plat.date_submit = dr["date_submitted"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_submitted"]);
                                    plat.num_order = dr["num_order"] == DBNull.Value ? 0 : Convert.ToInt32(dr["num_order"].ToString().Trim());
                                    plat.app_type_num = dr["app_type_num"] == DBNull.Value ? string.Empty : dr["app_type_num"].ToString().Trim();
                                    plat.decision_and_date = dr["decision_and_date"] == DBNull.Value ? string.Empty : dr["decision_and_date"].ToString().Trim();
                                    plat.summ_activity = dr["summ_activities"] == DBNull.Value ? string.Empty : dr["summ_activities"].ToString().Trim();

                                    items.Add(plat);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetPostAuthActivityListById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            return items;
        }
        public List<DecisionMilestone> GetDecisionMilestoneListById(string id)
        {
            var items = new List<DecisionMilestone>();

            string commandText = string.Empty;
            commandText = "SELECT link_id, num_order, milestone, completed_date, separator, completed_date2 FROM bd_milestones WHERE link_ID = @link_id";
            if (this.Lang.Equals("fr"))
            {
                commandText += " AND language='french';";
            }
            else
            {
                commandText += " AND language='english';";
            }

            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                    {
                        con.Open();
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var milestone = new DecisionMilestone();
                                    milestone.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    milestone.num_order = dr["num_order"] == DBNull.Value ? 0 : Convert.ToInt32(dr["num_order"].ToString().Trim());
                                    milestone.milestone = dr["milestone"] == DBNull.Value ? string.Empty : dr["milestone"].ToString().Trim();
                                    milestone.separator = dr["separator"] == DBNull.Value ? string.Empty : dr["separator"].ToString().Trim();
                                    milestone.completed_date = dr["completed_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["completed_date"]);
                                    milestone.completed_date2 = dr["completed_date2"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["completed_date2"]);
                                    items.Add(milestone);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetDecisionMilestoneListById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            return items;
        }


        public List<Tombstone> GetBasicDecisionTombstoneListById(string id)
        {
            var items = new List<Tombstone>();

            string commandText = string.Empty;
            commandText = "SELECT link_id, num_order, med_ingredient, nonprop_name, strength, dosageform, route_admin, thera_class, nonmed_ingredient FROM bd_tombstone WHERE link_id = @link_id";
            if (this.Lang.Equals("fr"))
            {
                commandText += " AND upper(language)='FRENCH';";
            }
            else
            {
                commandText += " AND upper(language)='ENGLISH';";
            }

            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                    {
                        con.Open();
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var tombstone = new Tombstone();
                                    tombstone.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    tombstone.num_order = dr["num_order"] == DBNull.Value ? 0 : Convert.ToInt32(dr["num_order"].ToString().Trim());
                                    tombstone.med_ingredient = dr["med_ingredient"] == DBNull.Value ? string.Empty : dr["med_ingredient"].ToString().Trim();
                                    tombstone.nonprop_name = dr["nonprop_name"] == DBNull.Value ? string.Empty : dr["nonprop_name"].ToString().Trim();
                                    tombstone.strength = dr["strength"] == DBNull.Value ? string.Empty : dr["strength"].ToString().Trim();
                                    tombstone.dosageform = dr["dosageform"] == DBNull.Value ? string.Empty : dr["dosageform"].ToString().Trim();
                                    tombstone.route_admin = dr["route_admin"] == DBNull.Value ? string.Empty : dr["route_admin"].ToString().Trim();
                                    tombstone.thera_class = dr["thera_class"] == DBNull.Value ? string.Empty : dr["thera_class"].ToString().Trim();
                                    tombstone.nonmed_ingredient = dr["nonmed_ingredient"] == DBNull.Value ? string.Empty : dr["nonmed_ingredient"].ToString().Trim();

                                    items.Add(tombstone);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetTombstoneListById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            return items;
        }
        public List<ApplicationMilestones> GetApplicationMilestoneListById(string id)
        {
            var items = new List<ApplicationMilestones>();

            string commandText = string.Empty;
            commandText = "SELECT link_id, num_order, application_milestone, milestone_date, milstone_date2, date_separator FROM sbd_med_milestone WHERE link_ID = @link_id AND ";
            if (this.Lang.Equals("fr"))
            {
                commandText += " upper(language)='FRENCH';";
            }
            else
            {
                commandText += " upper(language)='ENGLISH';";
            }
            

            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                    {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var milestone = new ApplicationMilestones();
                                    milestone.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    milestone.num_order = dr["num_order"] == DBNull.Value ? 0 : Convert.ToInt32(dr["num_order"].ToString().Trim());
                                    milestone.application_milestone = dr["application_milestone"] == DBNull.Value ? string.Empty : dr["application_milestone"].ToString().Trim();
                                    milestone.separator = dr["date_separator"] == DBNull.Value ? string.Empty : dr["date_separator"].ToString().Trim();
                                    milestone.milestone_date = dr["milestone_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["milestone_date"]);
                                    milestone.milestone_date2 = dr["milstone_date2"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["milstone_date2"]);
                                    items.Add(milestone);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetDecisionMilestoneListById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            return items;
        }
        public List<BullePoint> GetRegulatoryDecisioBulletListById(string id)
        {
            var items = new List<BullePoint>();

            string commandText = string.Empty;
            commandText = "SELECT num_order as orderNo,";
            if (this.Lang.Equals("fr"))
            {
                commandText += "bullet_value_fr as bullet";
            }
            else
            {
                commandText += "bullet_value_en as bullet";
            }
            commandText += " FROM rds_bullet_points WHERE link_ID = @link_id";

            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                    {
                        con.Open();
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var bullet = new BullePoint();
                                    bullet.field_id = 0;
                                    bullet.order_no = dr["orderNo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["orderNo"].ToString().Trim());
                                    bullet.bullet = dr["bullet"] == DBNull.Value ? string.Empty : dr["bullet"].ToString().Trim();
                                    items.Add(bullet);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetRegulatoryDecisioBulletListById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            return items;
        }


        public SafetyReview GetSafetyReviewById(string id)
        {
            var item = new SafetyReview();
            string commandText = string.Empty;
            commandText = "SELECT link_id, drug_name, created_date, review_date, modified_date, template,";
            if (this.Lang.Equals("fr"))
            {
                commandText += " safteyissue_fr as safteyissue, issue_fr as issue, background_fr as background,"
                            + " objective_fr as objective, key_findings_fr as key_findings,"
                            + " key_messages_fr as key_messages, overview_fr as overview, use_canada_fr as use_canada, findings_fr as findings,"
                            + " conclusion_fr as conclusion, additional_fr as additional, full_review_fr as full_review,"
                            + " sr_references_fr as sr_references, footnotes_fr as footnotes";
            }
            else
            {
                commandText += " safteyissue_en as safteyissue, issue_en as issue, background_en as background,"
                            + " objective_en as objective, key_findings_en as key_findings,"
                            + " key_messages_en as key_messages, overview_en as overview, use_canada_en as use_canada, findings_en as findings,"
                            + " conclusion_en as conclusion, additional_en as additional, full_review_en as full_review,"
                            + " sr_references_en as sr_references, footnotes_en as footnotes";
            }
            commandText += " FROM SSR WHERE link_ID = @link_id";

            using ( NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                    {
                        con.Open();
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    item.template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.review_date = dr["review_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["review_date"]);
                                    item.drug_name = dr["drug_name"] == DBNull.Value ? string.Empty : dr["drug_name"].ToString().Trim();
                                    item.safety_issue = dr["safteyissue"] == DBNull.Value ? string.Empty : dr["safteyissue"].ToString().Trim();
                                    item.issue = dr["issue"] == DBNull.Value ? string.Empty : dr["issue"].ToString().Trim();
                                    item.background = dr["background"] == DBNull.Value ? string.Empty : dr["background"].ToString().Trim();
                                    item.objective = dr["objective"] == DBNull.Value ? string.Empty : dr["objective"].ToString().Trim();
                                    item.key_findings = dr["key_findings"] == DBNull.Value ? string.Empty : dr["key_findings"].ToString().Trim();
                                    item.key_messages = dr["key_messages"] == DBNull.Value ? 0 : Convert.ToInt32(dr["key_messages"]);
                                    item.overview = dr["overview"] == DBNull.Value ? string.Empty : dr["overview"].ToString().Trim();
                                    item.use_canada = dr["use_canada"] == DBNull.Value ? 0 : Convert.ToInt32(dr["use_canada"]);
                                    item.findings = dr["findings"] == DBNull.Value ? 0 : Convert.ToInt32(dr["findings"]);
                                    item.conclusion = dr["conclusion"] == DBNull.Value ? 0 : Convert.ToInt32(dr["conclusion"]);
                                    item.additional = dr["additional"] == DBNull.Value ? string.Empty : dr["additional"].ToString().Trim();
                                    item.full_review = dr["full_review"] == DBNull.Value ? string.Empty : dr["full_review"].ToString().Trim();
                                    item.references = dr["sr_references"] == DBNull.Value ? 0 : Convert.ToInt32(dr["sr_references"]);
                                    item.footnotes = dr["footnotes"] == DBNull.Value ? 0 : Convert.ToInt32(dr["footnotes"]);
                                    item.created_date = dr["created_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["created_date"]); ;
                                    item.modified_date = dr["modified_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["modified_date"]);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetSafetyReviewById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            return item;
        }
        public List<RegulatoryDecision> GetAllRegulatoryDecision()
        {
            var items = new List<RegulatoryDecision>();
           
            string commandText = string.Empty;
            commandText = "SELECT link_id, drugname, manufacture, date_decision, modified_date, control_number, ";
            if ( this.Lang.Equals("fr"))
            {
                commandText += " type_submission_fr as type_submission, active_ingredient_fr as medical_ingredient, decision_fr as decision";
            }
            else
            {
                commandText += " type_submission_en as type_submission, active_ingredient_en as medical_ingredient, decision_en as decision";
            }
            commandText += " FROM RDS";


            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                    {
                        try
                        {
                            using (NpgsqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        var item = new RegulatoryDecision();
                                        item.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                        item.drug_name = dr["drugname"] == DBNull.Value ? string.Empty : dr["drugname"].ToString().Trim();
                                        item.date_decision = dr["date_decision"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_decision"]);
                                        item.decision = dr["decision"] == DBNull.Value ? string.Empty : dr["decision"].ToString().Trim();
                                        item.manufacturer = dr["manufacture"] == DBNull.Value ? string.Empty : dr["manufacture"].ToString().Trim();
                                        item.type_submission = dr["type_submission"] == DBNull.Value ? string.Empty : dr["type_submission"].ToString().Trim();
                                        item.control_number = dr["control_number"] == DBNull.Value ? 0 : Convert.ToInt32(dr["control_number"]);
                                        item.modified_date = dr["modified_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["modified_date"]);
                                        item.medical_ingredient = dr["medical_ingredient"] == DBNull.Value ? string.Empty : dr["medical_ingredient"].ToString().Trim();

                                        items.Add(item);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMessages = string.Format("DbConnection.cs - GetAllRegulatoryDecision()");
                            ExceptionHelper.LogException(ex, errorMessages);
                            Console.WriteLine(errorMessages);
                        }
                        finally
                        {
                            if (con.State == ConnectionState.Open)
                                con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessages = string.Format("DbConnection.cs - GetAllRegulatoryDecision()");
                ExceptionHelper.LogException(ex, errorMessages);
                Console.WriteLine(errorMessages);
            }
           
            return items;
        }

        public RegulatoryDecision GetRegulatoryDecisionById(string LinkID)
        {
            var returnItem = new RegulatoryDecision();
            var items = new List<RegulatoryDecision>();
            string commandText = string.Empty;
            commandText = "SELECT a.link_id, a.drugname, a.manufacture, a.date_decision, a.modified_date, a.date_filed, a.created_date, a.control_number, ";
            if (this.Lang.Equals("fr"))
            {
                commandText += " a.type_submission_fr as type_submission, a.active_ingredient_fr as medical_ingredient, a.contact_name_fr as contact_name, a.contact_url_fr as contact_url,"
                            + " a.therapeutic_area_fr as therapeutic_area, a.purpose_fr as purpose, a.reason_decision_fr as reason_decision, a.decision_fr as decision,"
                            + " a.decision_descr_fr as decision_descr, a.prescription_status_fr as prescription_status, a.footnotes_fr as footnotes";

            }
            else
            {
                commandText += "a.type_submission_en as type_submission, a.active_ingredient_en as medical_ingredient, a.contact_name_en as contact_name, a.contact_url_en as contact_url,"
                        + " a.therapeutic_area_en as therapeutic_area, a.purpose_en as purpose, a.reason_decision_en as reason_decision, a.decision_en as decision,"
                        + " a.decision_descr_en as decision_descr, a.prescription_status_en as prescription_status, a.footnotes_en as footnotes";
            }
                commandText += " FROM RDS as a"
                            + " WHERE a.link_ID = @link_id";
            using ( NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", LinkID.ToUpper().Trim());
                    try
                    {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new RegulatoryDecision();

                                    item.link_id = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.drug_name = dr["drugname"] == DBNull.Value ? string.Empty : dr["drugname"].ToString().Trim();
                                    item.contact_name = dr["contact_name"] == DBNull.Value ? string.Empty : dr["contact_name"].ToString().Trim();
                                    item.contact_url = dr["contact_url"] == DBNull.Value ? string.Empty : dr["contact_url"].ToString().Trim();
                                    item.medical_ingredient = dr["medical_ingredient"] == DBNull.Value ? string.Empty : dr["medical_ingredient"].ToString().Trim();
                                    item.therapeutic_area = dr["therapeutic_area"] == DBNull.Value ? string.Empty : dr["therapeutic_area"].ToString().Trim();
                                    item.purpose = dr["purpose"] == DBNull.Value ? string.Empty : dr["purpose"].ToString().Trim();
                                    item.reason_decision = dr["reason_decision"] == DBNull.Value ? string.Empty : dr["reason_decision"].ToString().Trim();
                                    item.decision = dr["decision"] == DBNull.Value ? string.Empty : dr["decision"].ToString().Trim();
                                    item.decision_descr = dr["decision_descr"] == DBNull.Value ? string.Empty : dr["decision_descr"].ToString().Trim();
                                    item.date_decision = dr["date_decision"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_decision"]);
                                    item.manufacturer = dr["manufacture"] == DBNull.Value ? string.Empty : dr["manufacture"].ToString().Trim();
                                    item.prescription_status = dr["prescription_status"] == DBNull.Value ? string.Empty : dr["prescription_status"].ToString().Trim();
                                    item.type_submission = dr["type_submission"] == DBNull.Value ? string.Empty : dr["type_submission"].ToString().Trim();
                                    item.date_filed = dr["date_filed"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_filed"]);
                                    item.control_number = dr["control_number"] == DBNull.Value ? 0 : Convert.ToInt32(dr["control_number"]);
                                    item.created_date = dr["created_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["created_date"]); ;
                                    item.modified_date = dr["modified_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["modified_date"]);
                                    item.footnotes = dr["footnotes"] == DBNull.Value ? 0 : Convert.ToInt32(dr["footnotes"]);
                                    items.Add(item);
                                }

                                if (items != null && items.Count > 0)
                                {
                                    returnItem = items.FirstOrDefault();
                                    returnItem.din_list = new List<string>();

                                    foreach (var tempDin in items)
                                    {
                                        if (!string.IsNullOrWhiteSpace(tempDin.din))
                                            returnItem.din_list.Add(tempDin.din);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetRegulatoryDecisionById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }

            }
            return returnItem;
        }

        public List<string> GetRegulatoryDinListById(string id)
        {
            var dinList = new List<string>();

            string commandText = string.Empty;
            commandText = "SELECT link_id, din FROM rd_din";
            if (this.Lang.Equals("fr"))
            {
                commandText += " WHERE link_id = @link_id and language='french'";
            }
            else
            {
                commandText += " WHERE link_id = @link_id and language='english'";
            }

            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                    {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var din = string.Empty;
                                    din = dr["din"] == DBNull.Value ? string.Empty : dr["din"].ToString().Trim();
                                    dinList.Add(din);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetRegulatoryDinListById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            return dinList;
        }


        public List<string> GetBasicDecisionDinListById(string id)
        {
            var dinList = new List<string>();

            string commandText = string.Empty;
            commandText = "SELECT link_id, din, num_order FROM bd_din ";
            if (this.Lang.Equals("fr"))
            { 
                commandText += "WHERE link_id = @link_id and language='french' ORDER BY num_order";
            }
            else
            {
                commandText += "WHERE link_id = @link_id and language='english' ORDER BY num_order";
            }
            

            using (NpgsqlConnection con = new NpgsqlConnection(RCDBConnection))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, con))
                {
                    cmd.Parameters.AddWithValue("@link_id", id.ToUpper().Trim());
                    try
                    {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var din = string.Empty;
                                    din = dr["din"] == DBNull.Value ? string.Empty : dr["din"].ToString().Trim();
                                    dinList.Add(din);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessages = string.Format("DbConnection.cs - GetBasicDecisionDinListById()");
                        ExceptionHelper.LogException(ex, errorMessages);
                        Console.WriteLine(errorMessages);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            return dinList;
        }
    }

}
