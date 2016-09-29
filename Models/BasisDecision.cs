using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
    public class BasisDecision
    {
        // From BasisDecision       
        public int Template { get; set; }
        public string ControlNum { get; set; }
        public DateTime? DateIssued { get; set; }
        public string LinkId { get; set; }
        public string Brandname { get; set; }
        public string Manufacturer { get; set; }
        public string MedIngredient { get; set; }
        public string NonpropName { get; set; }
        public string Strength { get; set; }
        public string Dosageform { get; set; }
        public string RouteAdmin { get; set; }
        public int BdDinList { get; set; }
        public string TheraClass { get; set; }
        public string NonmedIngredient { get; set; }
        public string SubTypeNum { get; set; }
        public DateTime? DateSubmission { get; set; }
        public DateTime? DateAuthorization { get; set; }
        public string NoticeDecision { get; set; }
        public string SciRegDecision { get; set; }
        public string QualityBasis { get; set; }
        public string NonclinBasis { get; set; }
        public string NonclinBasis2 { get; set; }
        public string ClinBasis { get; set; }
        public string ClinBasis2 { get; set; }
        public string ClinBasis3 { get; set; }
        public string BenefitRisk { get; set; }
        public string Radioisotope { get; set; }
        public string Summary { get; set; }
        public string WhatApproved { get; set; }
        public string WhyApproved { get; set; }
        public string StepsApproval { get; set; }
        public string AssessBasis { get; set; }
        public string FollowupMeasures { get; set; }
        public string PostAuth { get; set; }
        public string OtherInfo { get; set; }
        public string ASciRegDcision { get; set; }
        public string ScienceRationale { get; set; }
        public string AClinBasis { get; set; }
        public string AClinBasis2 { get; set; }
        public string ANonClinBasis { get; set; }
        public string ANonClinBasis2 { get; set; }
        public string BQualityBasis { get; set; }
        public string Contact { get; set; }
        public string Din { get; set; }
        public string LicenseNo { get; set; }
        public bool IsMd { get; set; }
        public List<string> DinList { get; set; }

        public List<PostAuthActivity> PostActivityList { get; set; }
        public List<DecisionMilestone> MilestoneList { get; set; }

    }
    public class PostAuthActivity
    {
        public string LinkId { get; set; }
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

    public class DecisionMilestone
    {

        public string LinkId { get; set; }
        public int NumOrder { get; set; }
        public string Milestone { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CompletedDate2 { get; set; }
        public string Separator { get; set; }
    }
}