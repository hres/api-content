using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace regContentWebApi.Models
{
    public class BasisDecisionMedicalDevice
    {
        // From BasisDecision       
        public int Template { get; set; }
        public string LinkId { get; set; }        
        public string DeviceName { get; set; }
        public string ApplicationNum { get; set; }        
        public string RecentActivity { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string SummaryBasisIntro { get; set; }
        public string WhatApproved { get; set; }
        public string WhyDeviceApproved { get; set; }
        public string StepsApprovalIntro { get; set; }
        public string FollowupMeasures { get; set; }
        public string PostLicenceActivity { get; set; }
        public string OtherInfo { get; set; }
        public string ScientificRationale { get; set; }
        public string ScientificRationale2 { get; set; }
        public string ScientificRationale3 { get; set; }
        public DateTime? DateSbdIssued { get; set; }
        
        public string Egalement { get; set; }
        public string Manufacturer { get; set; }
        public string MedicalDeviceGroup { get; set; }
        public string BiologicalMaterial { get; set; }
        
        public string CombinationProduct { get; set; }
        public string DrugMaterial { get; set; }
        public string ApplicationTypeAndNum { get; set; }
        public DateTime? DateLicenceIssued { get; set; }
        public string IntendedUse { get; set; }
        public string NoticeOfDecision { get; set; }
        public string SciRegBasisDecision1 { get; set; }
        public string SciRegBasisDecision2 { get; set; }
        public string SciRegBasisDecision3 { get; set; }
        public string ResponseToCondition { get; set; }
        
        public string Conclusion { get; set; }
        public string Recommendation { get; set; }
        public string LicenseNo { get; set; }

        public bool IsMd { get; set; }       

        public List<PostLicensingActivity> PlatList { get; set; }
        public List<ApplicationMilestones> AppMilestoneList { get; set; }

    }
public class PostLicensingActivity
{
    public string LinkId { get; set; }
    public DateTime? DateSubmit { get; set; }  
    
    public int NumOrder { get; set; }
    public string AppTypeNum { get; set; }     

    public string DecisionAndDate { get; set; }
    public string SummActivity { get; set; }
}

public class ApplicationMilestones
    {

    public string LinkId { get; set; }
    public int NumOrder { get; set; }
    public string ApplicationMilestone { get; set; }
    public DateTime? MilestoneDate { get; set; }
    public DateTime? MilestoneDate2 { get; set; }
    public string Separator { get; set; }
}
}
