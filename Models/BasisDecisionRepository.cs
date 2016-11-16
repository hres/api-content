using regContentWebApi;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace regContentWebApi.Models
{
    public class BasisDecisionRepository : IBasisDecisionRepository
    {
        private List<BasisDecision> _basisDecisions = new List<BasisDecision>();
        private BasisDecision _basisDecision = new BasisDecision();
        private BasisDecisionMedicalDevice _basisDecisionMd = new BasisDecisionMedicalDevice();

        public IEnumerable<BasisDecision> GetAll(string lang)
        {
            DBConnection dbConnection = new DBConnection(lang);
            _basisDecisions = dbConnection.GetAllBasisDecision();

            if (_basisDecisions != null && _basisDecisions.Count > 0)
            {
                foreach (var item in _basisDecisions)
                {
                    if (!item.is_md)
                    {
                        item.din_list = new List<string>();
                        item.med_ingredient = string.Empty;
                        //To get din list
                        item.din_list = dbConnection.GetBasicDecisionDinListById(item.link_id);
                        //To get tombstone
                        item.tombstone_list = dbConnection.GetBasicDecisionTombstoneListById(item.link_id);
                        if(item.tombstone_list != null && item.tombstone_list.Count > 0)
                        {
                            var sb = new StringBuilder();
                            foreach ( var list in item.tombstone_list)
                            {
                                if(!string.IsNullOrWhiteSpace(list.med_ingredient))
                                {
                                    sb.AppendFormat("{0},", list.med_ingredient.Trim());
                                }
                            }
                            item.med_ingredient = sb.ToString().TrimEnd(',');
                        }
                    }
                }
            }

            var _basisDecisionsMedicalDevices = new List<BasisDecision>();
            _basisDecisionsMedicalDevices = dbConnection.GetAllBasisDecisionMedicalDevice();

            if (_basisDecisionsMedicalDevices != null && _basisDecisionsMedicalDevices.Count > 0)
            {
                _basisDecisions.AddRange(_basisDecisionsMedicalDevices);
            }
           // _basisDecisions.OrderByDescending(x => x.brand_name);
            return _basisDecisions;
        }

        public BasisDecision Get(string lang, string id)
        {
            DBConnection dbConnection = new DBConnection(lang);
            _basisDecision = dbConnection.GetBasisDecisionById(id);
            _basisDecision.post_activity_list = new List<PostAuthActivity>();
            _basisDecision.milestone_list = new List<DecisionMilestone>();
            _basisDecision.tombstone_list = new List<Tombstone>();
            if (_basisDecision != null && !string.IsNullOrEmpty(_basisDecision.link_id))
            {
                var paatList = dbConnection.GetPostAuthActivityListById(id);
                _basisDecision.din_list = dbConnection.GetBasicDecisionDinListById(id);
                if (paatList != null && paatList.Count > 0)
                {
                    _basisDecision.post_activity_list = paatList;                   
                }
                //Milestones.
                var mileList = dbConnection.GetDecisionMilestoneListById(id);
                if (mileList != null && mileList.Count > 0)
                {
                    _basisDecision.milestone_list = mileList;
                }
                //Tombstone
                var tombstoneList = dbConnection.GetBasicDecisionTombstoneListById(id);
                if (tombstoneList != null && tombstoneList.Count > 0)
                {
                    _basisDecision.tombstone_list = tombstoneList;
                }
            }
            return _basisDecision;
        }

        public BasisDecisionMedicalDevice GetMedicalDevice(string lang, string id)
        {
            DBConnection dbConnection = new DBConnection(lang);
            _basisDecisionMd = dbConnection.GetBasisDecisionMedicalDeviceById(id);
            _basisDecisionMd.plat_list = new List<PostLicensingActivity>();
            _basisDecisionMd.app_milestone_list = new List<ApplicationMilestones>();
            if (_basisDecisionMd != null && !string.IsNullOrEmpty(_basisDecisionMd.link_id))
            {
                var platList = dbConnection.GetPostLicensingActivityListById(id);
                if (platList != null && platList.Count > 0)
                {
                    _basisDecisionMd.plat_list = platList;
                }
                //Milestones.
                var mileList = dbConnection.GetApplicationMilestoneListById(id);
                if (mileList != null && mileList.Count > 0)
                {
                    _basisDecisionMd.app_milestone_list = mileList;
                }
            }
            return _basisDecisionMd;
        }
    }
}