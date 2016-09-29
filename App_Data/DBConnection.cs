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
            commandText = "SELECT link_id, brandname, manufacturer, date_issued, control_num, template, ";
            if (this.Lang.Equals("fr"))
            {
                commandText += " med_ingredient_fr as med_ingredient";
            }
            else
            {
                commandText += " med_ingredient_en as med_ingredient";
            }
            commandText += " FROM SBD";

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
                                    item.ControlNum = dr["control_num"] == DBNull.Value ? string.Empty : dr["control_num"].ToString().Trim();
                                    item.DateIssued = dr["date_issued"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_issued"]);
                                    item.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.Brandname = dr["brandname"] == DBNull.Value ? string.Empty : dr["brandname"].ToString().Trim();
                                    item.Manufacturer = dr["manufacturer"] == DBNull.Value ? string.Empty : dr["manufacturer"].ToString().Trim();
                                    item.MedIngredient = dr["med_ingredient"] == DBNull.Value ? string.Empty : dr["med_ingredient"].ToString().Trim();
                                    item.Template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.IsMd = false;
                                    item.LicenseNo = "";
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
                                    item.ControlNum = dr["application_num"] == DBNull.Value ? string.Empty : dr["application_num"].ToString().Trim();
                                    item.DateIssued = dr["updated_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["updated_date"]);
                                    item.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.Brandname = dr["device_name"] == DBNull.Value ? string.Empty : dr["device_name"].ToString().Trim();
                                    item.Manufacturer = dr["manufacturer"] == DBNull.Value ? string.Empty : dr["manufacturer"].ToString().Trim();
                                    item.MedIngredient = "N/A";
                                    item.Template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.IsMd = true;
                                    item.LicenseNo = dr["licence_num"] == DBNull.Value ? string.Empty : dr["licence_num"].ToString().Trim();
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
            commandText = "SELECT a.link_id, a.template, a.bd_din_list, a.date_submission, a.date_authorization, a.brandname, a.manufacturer, a.date_issued, a.control_num, ";

            if (this.Lang.Equals("fr"))
            {
                commandText += "a.nonprop_name_fr as nonprop_name, a.strength_fr as strength, a.dosageform_fr as dosageform,"
                            + " a.route_admin_fr as route_admin, a.thera_class_fr as thera_class, a.med_ingredient_fr as med_ingredient,"
                            + " a.nonmed_ingredient_fr as nonmed_ingredient, a.sub_type_num_fr as sub_type_num, a.notice_decision_fr as notice_decision,"
                            + " a.sci_reg_decision_fr as sci_reg_decision, a.quality_basis_fr as quality_basis, a.nonclin_basis_fr as nonclin_basis,"
                            + " a.nonclin_basis2_fr as nonclin_basis2, a.clin_basis_fr as clin_basis, a.clin_basis2_fr as clin_basis2, a.clin_basis3_fr as clin_basis3,"
                            + " a.benefit_risk_fr as benefit_risk, a.radioisotope_fr as radioisotope, a.summary_fr as summary,"
                            + " a.what_approved_fr as what_approved, a.why_approved_fr as why_approved, a.steps_approval_fr as steps_approval, a.assess_basis_fr as assess_basis, "
                            + " a.followup_measures_fr as followup_measures, "
                            + " a.post_auth_fr as post_auth, other_info_fr as other_info, a.a_sci_reg_decision_fr as a_sci_reg_decision,"
                            + " a.science_rationale_fr as science_rationale, a.a_clin_basis_fr as a_clin_basis, a.a_clin_basis2_fr as a_clin_basis2,"
                            + " a.a_non_clin_basis_fr as a_non_clin_basis, a.a_non_clin_basis2_fr as a_non_clin_basis2, a.b_quality_basis_fr as b_quality_basis, a.contact_fr as contact";
                            

            }
            else
            {
                commandText += " a.nonprop_name_en as nonprop_name, a.strength_en as strength, a.dosageform_en as dosageform,"
                            + " a.route_admin_en as route_admin, a.thera_class_en as thera_class, a.med_ingredient_en as med_ingredient,"
                            + " a.nonmed_ingredient_en as nonmed_ingredient, a.sub_type_num_en as sub_type_num, a.notice_decision_en as notice_decision,"
                            + " a.sci_reg_decision_en as sci_reg_decision, a.quality_basis_en as quality_basis, a.nonclin_basis_en as nonclin_basis,"
                            + " a.nonclin_basis2_en as nonclin_basis2, a.clin_basis_en as clin_basis, a.clin_basis2_en as clin_basis2, a.clin_basis3_en as clin_basis3,"
                            + " a.benefit_risk_en as benefit_risk, a.radioisotope_en as radioisotope, a.summary_en as summary,"
                            + " a.what_approved_en as what_approved, a.why_approved_en as why_approved, a.steps_approval_en as steps_approval, a.assess_basis_en as assess_basis, "
                            + " a.followup_measures_en as followup_measures, "
                            + " a.post_auth_en as post_auth, other_info_en as other_info, a.a_sci_reg_decision_en as a_sci_reg_decision,"
                            + " a.science_rationale_en as science_rationale, a.a_clin_basis_en as a_clin_basis, a.a_clin_basis2_en as a_clin_basis2,"
                            + " a.a_non_clin_basis_en as a_non_clin_basis, a.a_non_clin_basis2_en as a_non_clin_basis2, a.b_quality_basis_en as b_quality_basis, a.contact_en as contact";
            }
                 commandText += " FROM SBD as a"                            
                             + " WHERE a.link_ID = @link_id";
            

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
                                    item.Template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.ControlNum = dr["control_num"] == DBNull.Value ? string.Empty : dr["control_num"].ToString().Trim();
                                    item.DateIssued = dr["date_issued"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_issued"]);
                                    item.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.Brandname = dr["brandname"] == DBNull.Value ? string.Empty : dr["brandname"].ToString().Trim();
                                    item.Manufacturer = dr["manufacturer"] == DBNull.Value ? string.Empty : dr["manufacturer"].ToString().Trim();
                                    item.MedIngredient = dr["med_ingredient"] == DBNull.Value ? string.Empty : dr["med_ingredient"].ToString().Trim();
                                    item.NonpropName = dr["nonprop_name"] == DBNull.Value ? string.Empty : dr["nonprop_name"].ToString().Trim();
                                    item.Strength = dr["strength"] == DBNull.Value ? string.Empty : dr["strength"].ToString().Trim();
                                    item.Dosageform = dr["dosageform"] == DBNull.Value ? string.Empty : dr["dosageform"].ToString().Trim();
                                    item.RouteAdmin = dr["route_admin"] == DBNull.Value ? string.Empty : dr["route_admin"].ToString().Trim();
                                    item.BdDinList = dr["bd_din_list"] == DBNull.Value ? 0 : Convert.ToInt32(dr["bd_din_list"]);
                                    item.TheraClass = dr["thera_class"] == DBNull.Value ? string.Empty : dr["thera_class"].ToString().Trim();
                                    item.NonmedIngredient = dr["nonmed_ingredient"] == DBNull.Value ? string.Empty : dr["nonmed_ingredient"].ToString().Trim();
                                    item.SubTypeNum = dr["sub_type_num"] == DBNull.Value ? string.Empty : dr["sub_type_num"].ToString().Trim();
                                    item.DateSubmission = dr["date_submission"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_submission"]);
                                    item.DateAuthorization = dr["date_authorization"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_authorization"]);
                                    item.NoticeDecision = dr["notice_decision"] == DBNull.Value ? string.Empty : dr["notice_decision"].ToString().Trim();
                                    item.SciRegDecision = dr["sci_reg_decision"] == DBNull.Value ? string.Empty : dr["sci_reg_decision"].ToString().Trim();
                                    item.QualityBasis = dr["quality_basis"] == DBNull.Value ? string.Empty : dr["quality_basis"].ToString().Trim();
                                    item.NonclinBasis = dr["nonclin_basis"] == DBNull.Value ? string.Empty : dr["nonclin_basis"].ToString().Trim();
                                    item.NonclinBasis2 = dr["nonclin_basis2"] == DBNull.Value ? string.Empty : dr["nonclin_basis2"].ToString().Trim();
                                    item.ClinBasis = dr["clin_basis"] == DBNull.Value ? string.Empty : dr["clin_basis"].ToString().Trim();
                                    item.ClinBasis2 = dr["clin_basis2"] == DBNull.Value ? string.Empty : dr["clin_basis2"].ToString().Trim();
                                    item.ClinBasis3 = dr["clin_basis3"] == DBNull.Value ? string.Empty : dr["clin_basis3"].ToString().Trim();
                                    item.BenefitRisk = dr["benefit_risk"] == DBNull.Value ? string.Empty : dr["benefit_risk"].ToString().Trim();
                                    item.Radioisotope = dr["radioisotope"] == DBNull.Value ? string.Empty : dr["radioisotope"].ToString().Trim();
                                    item.Summary = dr["summary"] == DBNull.Value ? string.Empty : dr["summary"].ToString().Trim();
                                    item.WhatApproved = dr["what_approved"] == DBNull.Value ? string.Empty : dr["what_approved"].ToString().Trim();
                                    item.WhyApproved = dr["why_approved"] == DBNull.Value ? string.Empty : dr["why_approved"].ToString().Trim();
                                    item.StepsApproval = dr["steps_approval"] == DBNull.Value ? string.Empty : dr["steps_approval"].ToString().Trim();
                                    item.AssessBasis = dr["assess_basis"] == DBNull.Value ? string.Empty : dr["assess_basis"].ToString().Trim();
                                    item.FollowupMeasures = dr["followup_measures"] == DBNull.Value ? string.Empty : dr["followup_measures"].ToString().Trim();
                                    item.PostAuth = dr["post_auth"] == DBNull.Value ? string.Empty : dr["post_auth"].ToString().Trim();
                                    item.OtherInfo = dr["other_info"] == DBNull.Value ? string.Empty : dr["other_info"].ToString().Trim();
                                    item.ASciRegDcision = dr["a_sci_reg_decision"] == DBNull.Value ? string.Empty : dr["a_sci_reg_decision"].ToString().Trim();
                                    item.ScienceRationale = dr["science_rationale"] == DBNull.Value ? string.Empty : dr["science_rationale"].ToString().Trim();
                                    item.AClinBasis = dr["a_clin_basis"] == DBNull.Value ? string.Empty : dr["a_clin_basis"].ToString().Trim();
                                    item.AClinBasis2 = dr["a_clin_basis2"] == DBNull.Value ? string.Empty : dr["a_clin_basis2"].ToString().Trim();
                                    item.ANonClinBasis = dr["a_non_clin_basis"] == DBNull.Value ? string.Empty : dr["a_non_clin_basis"].ToString().Trim();
                                    item.ANonClinBasis2 = dr["a_non_clin_basis2"] == DBNull.Value ? string.Empty : dr["a_non_clin_basis2"].ToString().Trim();
                                    item.BQualityBasis = dr["b_quality_basis"] == DBNull.Value ? string.Empty : dr["b_quality_basis"].ToString().Trim();
                                    item.Contact = dr["contact"] == DBNull.Value ? string.Empty : dr["contact"].ToString().Trim();
                                    items.Add(item);
                                }

                                if (items != null && items.Count > 0)
                                {
                                    returnItem = items.FirstOrDefault();
                                    returnItem.DinList = new List<string>();

                                    foreach (var tempDin in items)
                                    {
                                        if (!string.IsNullOrWhiteSpace(tempDin.Din))
                                            returnItem.DinList.Add(tempDin.Din);
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
                                    item.Template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.DeviceName = dr["device_name"] == DBNull.Value ? string.Empty : dr["device_name"].ToString().Trim();
                                    item.ApplicationNum = dr["application_num"] == DBNull.Value ? string.Empty : dr["application_num"].ToString().Trim();
                                    item.RecentActivity = dr["recent_activity"] == DBNull.Value ? string.Empty : dr["recent_activity"].ToString().Trim();
                                    item.UpdatedDate = dr["updated_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["updated_date"]);
                                    item.SummaryBasisIntro = dr["summary_basis_intro"] == DBNull.Value ? string.Empty : dr["summary_basis_intro"].ToString().Trim();
                                    item.WhatApproved = dr["what_approved"] == DBNull.Value ? string.Empty : dr["what_approved"].ToString().Trim();
                                    item.WhyDeviceApproved = dr["why_device_approved"] == DBNull.Value ? string.Empty : dr["why_device_approved"].ToString().Trim();
                                    item.StepsApprovalIntro = dr["steps_approval_intro"] == DBNull.Value ? string.Empty : dr["steps_approval_intro"].ToString().Trim();
                                    item.FollowupMeasures = dr["followup_measures"] == DBNull.Value ? string.Empty : dr["followup_measures"].ToString().Trim();
                                    item.PostLicenceActivity = dr["post_licence_activity"] == DBNull.Value ? string.Empty : dr["post_licence_activity"].ToString().Trim();
                                    item.OtherInfo = dr["other_info"] == DBNull.Value ? string.Empty : dr["other_info"].ToString().Trim();
                                    item.ScientificRationale = dr["scientific_rationale"] == DBNull.Value ? string.Empty : dr["scientific_rationale"].ToString().Trim();
                                    item.ScientificRationale2 = dr["scientific_rationale2"] == DBNull.Value ? string.Empty : dr["scientific_rationale2"].ToString().Trim();
                                    item.ScientificRationale3 = dr["scientific_rationale3"] == DBNull.Value ? string.Empty : dr["scientific_rationale3"].ToString().Trim();
                                    item.DateSbdIssued = dr["date_sbd_issued"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_sbd_issued"]);
                                    item.Egalement = dr["egalement"] == DBNull.Value ? string.Empty : dr["egalement"].ToString().Trim();
                                    item.Manufacturer = dr["manufacturer"] == DBNull.Value ? string.Empty : dr["manufacturer"].ToString().Trim();
                                    item.MedicalDeviceGroup = dr["medical_device_group"] == DBNull.Value ? string.Empty : dr["medical_device_group"].ToString().Trim();
                                    item.BiologicalMaterial = dr["biological_material"] == DBNull.Value ? string.Empty : dr["biological_material"].ToString().Trim();
                                    item.CombinationProduct = dr["combination_product"] == DBNull.Value ? string.Empty : dr["combination_product"].ToString().Trim();
                                    item.DrugMaterial = dr["drug_material"] == DBNull.Value ? string.Empty : dr["drug_material"].ToString().Trim();
                                    item.ApplicationTypeAndNum = dr["application_type_and_num"] == DBNull.Value ? string.Empty : dr["application_type_and_num"].ToString().Trim();
                                    item.DateLicenceIssued = dr["date_licence_issued"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_licence_issued"]);
                                    item.IntendedUse = dr["intended_use"] == DBNull.Value ? string.Empty : dr["intended_use"].ToString().Trim();
                                    item.NoticeOfDecision = dr["notice_of_decision"] == DBNull.Value ? string.Empty : dr["notice_of_decision"].ToString().Trim();
                                    item.SciRegBasisDecision1 = dr["sci_reg_basis_decision1"] == DBNull.Value ? string.Empty : dr["sci_reg_basis_decision1"].ToString().Trim();
                                    item.SciRegBasisDecision2 = dr["sci_reg_basis_decision2"] == DBNull.Value ? string.Empty : dr["sci_reg_basis_decision2"].ToString().Trim();
                                    item.SciRegBasisDecision3 = dr["sci_reg_basis_decision3"] == DBNull.Value ? string.Empty : dr["sci_reg_basis_decision3"].ToString().Trim();
                                    item.ResponseToCondition = dr["response_to_condition"] == DBNull.Value ? string.Empty : dr["response_to_condition"].ToString().Trim();
                                    item.Conclusion = dr["conclusion"] == DBNull.Value ? string.Empty : dr["conclusion"].ToString().Trim();
                                    item.Recommendation = dr["recommendation"] == DBNull.Value ? string.Empty : dr["recommendation"].ToString().Trim();
                                    item.LicenseNo = dr["licence_num"] == DBNull.Value ? string.Empty : dr["licence_num"].ToString().Trim();
                                    item.IsMd = true;
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
                                    item.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.Template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.DrugName = dr["drug_name"] == DBNull.Value ? string.Empty : dr["drug_name"].ToString().Trim();
                                    item.Safetyissue = dr["safteyissue"] == DBNull.Value ? string.Empty : dr["safteyissue"].ToString().Trim();
                                    item.CreatedDate = dr["created_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["created_date"]); ;
                                    item.ModifiedDate = dr["modified_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["modified_date"]);
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
                                    bullet.FieldId = dr["fieldID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["fieldID"].ToString().Trim());
                                    bullet.OrderNo = dr["orderNo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["orderNo"].ToString().Trim());
                                    bullet.Bullet = dr["bullet"] == DBNull.Value ? string.Empty : dr["bullet"].ToString().Trim();
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
                                        paat.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                        paat.RowNum = dr["row_num"] == DBNull.Value ? 0 : Convert.ToInt32(dr["row_num"].ToString().Trim());
                                        paat.ActContrNum = dr["act_contr_num"] == DBNull.Value ? string.Empty : dr["act_contr_num"].ToString().Trim();
                                        paat.DateSubmit = dr["date_submit"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_submit"]);
                                        paat.SubmitText = dr["date_submit_text"] == DBNull.Value ? string.Empty : dr["date_submit_text"].ToString().Trim();
                                        paat.PaatDecision = dr["paat_decision"] == DBNull.Value ? string.Empty : dr["paat_decision"].ToString().Trim();
                                        paat.DecisionStartDate = dr["paat_decision_start_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["paat_decision_start_date"]);
                                        paat.DateText = dr["paat_date_text"] == DBNull.Value ? string.Empty : dr["paat_date_text"].ToString().Trim();
                                        paat.DecisionEndDate = dr["paat_decision_end_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["paat_decision_end_date"]);
                                        paat.SummActivity = dr["summ_activ"] == DBNull.Value ? string.Empty : dr["summ_activ"].ToString().Trim();
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
                                    plat.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    plat.DateSubmit = dr["date_submitted"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_submitted"]);
                                    plat.NumOrder = dr["num_order"] == DBNull.Value ? 0 : Convert.ToInt32(dr["num_order"].ToString().Trim());
                                    plat.AppTypeNum = dr["app_type_num"] == DBNull.Value ? string.Empty : dr["app_type_num"].ToString().Trim();
                                    plat.DecisionAndDate = dr["decision_and_date"] == DBNull.Value ? string.Empty : dr["decision_and_date"].ToString().Trim();
                                    plat.SummActivity = dr["summ_activities"] == DBNull.Value ? string.Empty : dr["summ_activities"].ToString().Trim();

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
                                    milestone.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    milestone.NumOrder = dr["num_order"] == DBNull.Value ? 0 : Convert.ToInt32(dr["num_order"].ToString().Trim());
                                    milestone.Milestone = dr["milestone"] == DBNull.Value ? string.Empty : dr["milestone"].ToString().Trim();
                                    milestone.Separator = dr["separator"] == DBNull.Value ? string.Empty : dr["separator"].ToString().Trim();
                                    milestone.CompletedDate = dr["completed_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["completed_date"]);
                                    milestone.CompletedDate2 = dr["completed_date2"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["completed_date2"]);
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
                                    milestone.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    milestone.NumOrder = dr["num_order"] == DBNull.Value ? 0 : Convert.ToInt32(dr["num_order"].ToString().Trim());
                                    milestone.ApplicationMilestone = dr["application_milestone"] == DBNull.Value ? string.Empty : dr["application_milestone"].ToString().Trim();
                                    milestone.Separator = dr["date_separator"] == DBNull.Value ? string.Empty : dr["date_separator"].ToString().Trim();
                                    milestone.MilestoneDate = dr["milestone_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["milestone_date"]);
                                    milestone.MilestoneDate2 = dr["milstone_date2"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["milstone_date2"]);
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
                                    bullet.FieldId = 0;
                                    bullet.OrderNo = dr["orderNo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["orderNo"].ToString().Trim());
                                    bullet.Bullet = dr["bullet"] == DBNull.Value ? string.Empty : dr["bullet"].ToString().Trim();
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
                                    item.Template = dr["template"] == DBNull.Value ? 0 : Convert.ToInt32(dr["template"]);
                                    item.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.ReviewDate = dr["review_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["review_date"]);
                                    item.DrugName = dr["drug_name"] == DBNull.Value ? string.Empty : dr["drug_name"].ToString().Trim();
                                    item.Safetyissue = dr["safteyissue"] == DBNull.Value ? string.Empty : dr["safteyissue"].ToString().Trim();
                                    item.Issue = dr["issue"] == DBNull.Value ? string.Empty : dr["issue"].ToString().Trim();
                                    item.Background = dr["background"] == DBNull.Value ? string.Empty : dr["background"].ToString().Trim();
                                    item.Objective = dr["objective"] == DBNull.Value ? string.Empty : dr["objective"].ToString().Trim();
                                    item.KeyFindings = dr["key_findings"] == DBNull.Value ? string.Empty : dr["key_findings"].ToString().Trim();
                                    item.KeyMessages = dr["key_messages"] == DBNull.Value ? 0 : Convert.ToInt32(dr["key_messages"]);
                                    item.Overview = dr["overview"] == DBNull.Value ? string.Empty : dr["overview"].ToString().Trim();
                                    item.UseCanada = dr["use_canada"] == DBNull.Value ? 0 : Convert.ToInt32(dr["use_canada"]);
                                    item.Findings = dr["findings"] == DBNull.Value ? 0 : Convert.ToInt32(dr["findings"]);
                                    item.Conclusion = dr["conclusion"] == DBNull.Value ? 0 : Convert.ToInt32(dr["conclusion"]);
                                    item.Additional = dr["additional"] == DBNull.Value ? string.Empty : dr["additional"].ToString().Trim();
                                    item.FullReview = dr["full_review"] == DBNull.Value ? string.Empty : dr["full_review"].ToString().Trim();
                                    item.References = dr["sr_references"] == DBNull.Value ? 0 : Convert.ToInt32(dr["sr_references"]);
                                    item.Footnotes = dr["footnotes"] == DBNull.Value ? 0 : Convert.ToInt32(dr["footnotes"]);
                                    item.CreatedDate = dr["created_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["created_date"]); ;
                                    item.ModifiedDate = dr["modified_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["modified_date"]);
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
                                        item.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                        item.Drugname = dr["drugname"] == DBNull.Value ? string.Empty : dr["drugname"].ToString().Trim();
                                        item.DateDecision = dr["date_decision"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_decision"]);
                                        item.Decision = dr["decision"] == DBNull.Value ? string.Empty : dr["decision"].ToString().Trim();
                                        item.Manufacture = dr["manufacture"] == DBNull.Value ? string.Empty : dr["manufacture"].ToString().Trim();
                                        item.TypeSubmission = dr["type_submission"] == DBNull.Value ? string.Empty : dr["type_submission"].ToString().Trim();
                                        item.ControlNumber = dr["control_number"] == DBNull.Value ? 0 : Convert.ToInt32(dr["control_number"]);
                                        item.ModifiedDate = dr["modified_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["modified_date"]);
                                        item.MedicalIngredient = dr["medical_ingredient"] == DBNull.Value ? string.Empty : dr["medical_ingredient"].ToString().Trim();

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

                                    item.LinkId = dr["link_id"] == DBNull.Value ? string.Empty : dr["link_id"].ToString().Trim();
                                    item.Drugname = dr["drugname"] == DBNull.Value ? string.Empty : dr["drugname"].ToString().Trim();
                                    item.ContactName = dr["contact_name"] == DBNull.Value ? string.Empty : dr["contact_name"].ToString().Trim();
                                    item.ContactUrl = dr["contact_url"] == DBNull.Value ? string.Empty : dr["contact_url"].ToString().Trim();
                                    item.MedicalIngredient = dr["medical_ingredient"] == DBNull.Value ? string.Empty : dr["medical_ingredient"].ToString().Trim();
                                    item.TherapeuticArea = dr["therapeutic_area"] == DBNull.Value ? string.Empty : dr["therapeutic_area"].ToString().Trim();
                                    item.Purpose = dr["purpose"] == DBNull.Value ? string.Empty : dr["purpose"].ToString().Trim();
                                    item.ReasonDecision = dr["reason_decision"] == DBNull.Value ? string.Empty : dr["reason_decision"].ToString().Trim();
                                    item.Decision = dr["decision"] == DBNull.Value ? string.Empty : dr["decision"].ToString().Trim();
                                    item.DecisionDescr = dr["decision_descr"] == DBNull.Value ? string.Empty : dr["decision_descr"].ToString().Trim();
                                    item.DateDecision = dr["date_decision"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_decision"]);
                                    item.Manufacture = dr["manufacture"] == DBNull.Value ? string.Empty : dr["manufacture"].ToString().Trim();
                                    item.PrescriptionStatus = dr["prescription_status"] == DBNull.Value ? string.Empty : dr["prescription_status"].ToString().Trim();
                                    item.TypeSubmission = dr["type_submission"] == DBNull.Value ? string.Empty : dr["type_submission"].ToString().Trim();
                                    item.DateFiled = dr["date_filed"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_filed"]);
                                    item.ControlNumber = dr["control_number"] == DBNull.Value ? 0 : Convert.ToInt32(dr["control_number"]);
                                    item.CreatedDate = dr["created_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["created_date"]); ;
                                    item.ModifiedDate = dr["modified_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["modified_date"]);
                                    item.Footnotes = dr["footnotes"] == DBNull.Value ? 0 : Convert.ToInt32(dr["footnotes"]);
                                    items.Add(item);
                                }

                                if (items != null && items.Count > 0)
                                {
                                    returnItem = items.FirstOrDefault();
                                    returnItem.DinList = new List<string>();

                                    foreach (var tempDin in items)
                                    {
                                        if (!string.IsNullOrWhiteSpace(tempDin.Din))
                                            returnItem.DinList.Add(tempDin.Din);
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
