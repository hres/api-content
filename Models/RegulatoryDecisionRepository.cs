using regContentWebApi;
using System.Collections.Generic;
using System.Linq;

namespace regContentWebApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RegulatoryDecisionRepository : IRegulatoryDecisionRepository
    {
        private List<RegulatoryDecision> _regDecisions = new List<RegulatoryDecision>();
        private RegulatoryDecision _regDecision = new RegulatoryDecision();
        public IEnumerable<RegulatoryDecision> GetAll(string lang)
        {
            DBConnection dbConnection = new DBConnection(lang);
            _regDecisions = dbConnection.GetAllRegulatoryDecision();
            if (_regDecisions != null && _regDecisions.Count > 0)              
            {
                foreach ( var item in _regDecisions)
                {
                    item.din_list = new List<string>(); //To get din list
                    item.din_list = dbConnection.GetRegulatoryDinListById(item.link_id);    
               }                
            }
            //RDS_Devices
            var rdsDedicalDevices = new List<RegulatoryDecisionMedicalDevice>();
            rdsDedicalDevices = dbConnection.GetAllRegulatoryDecisionMedicalDevices();

            if (rdsDedicalDevices != null && rdsDedicalDevices.Count > 0)
            {
                foreach (var item in rdsDedicalDevices)
                {
                    var newItem = new RegulatoryDecision();
                    newItem.is_md = true;
                    newItem.link_id = item.link_id;
                    newItem.drug_name = item.drug_name;
                    newItem.medical_ingredient = item.medical_ingredient;
                    newItem.manufacturer = item.manufacturer;
                    newItem.decision = item.decision;
                    newItem.date_decision = item.date_decision;
                    newItem.control_number = item.application_number;
                    newItem.type_submission = item.type_application;
                    _regDecisions.Add(newItem);
                }
            }
         
            return _regDecisions;
        }

        public RegulatoryDecision Get(string lang, string id)
        {
            DBConnection dbConnection = new DBConnection(lang);
            _regDecision = dbConnection.GetRegulatoryDecisionById(id);
            if (_regDecision != null && !string.IsNullOrEmpty(_regDecision.link_id))
            {
                    
                _regDecision.din_list = dbConnection.GetRegulatoryDinListById(id);
                _regDecision.bullet_list = new List<BullePoint>();
                var bulletList = dbConnection.GetRegulatoryDecisioBulletListById(id);
                if (bulletList != null && bulletList.Count > 0)
                {
                    //footnotes ( will be uncommented once field_id added in the db)
                    _regDecision.bullet_list = bulletList.OrderBy(c => c.order_no).ToList();
                }
            }
            return _regDecision;
        }


        public RegulatoryDecisionMedicalDevice GetMedicalDevice(string lang, string id)
        {
            DBConnection dbConnection = new DBConnection(lang);
            var rdsMedicalDevic = dbConnection.GetRegulatoryDecisionMedicalDevicesById(id);
            return rdsMedicalDevic;
        }

        public IEnumerable<RegulatoryDecisionMedicalDevice> GetAllMedicalDevice(string lang)
        {
            DBConnection dbConnection = new DBConnection(lang);
            var rdsDedicalDevices = new List<RegulatoryDecisionMedicalDevice>();
            rdsDedicalDevices = dbConnection.GetAllRegulatoryDecisionMedicalDevices();
            return rdsDedicalDevices;
        }
    }
}