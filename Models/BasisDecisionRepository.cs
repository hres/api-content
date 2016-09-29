using regContentWebApi;
using System.Collections.Generic;
using System.Linq;

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

                    if (!item.IsMd)
                    {
                        //To get din list
                        item.DinList = dbConnection.GetBasicDecisionDinListById(item.LinkId);
                    }
                }
            }

            var _basisDecisionsMedicalDevices = new List<BasisDecision>();
            _basisDecisionsMedicalDevices = dbConnection.GetAllBasisDecisionMedicalDevice();

            if (_basisDecisionsMedicalDevices != null && _basisDecisionsMedicalDevices.Count > 0)
            {
                _basisDecisions.AddRange(_basisDecisionsMedicalDevices);
            }
            return _basisDecisions;
        }

        public BasisDecision Get(string lang, string id)
        {
            DBConnection dbConnection = new DBConnection(lang);
            _basisDecision = dbConnection.GetBasisDecisionById(id);
            _basisDecision.PostActivityList = new List<PostAuthActivity>();
            _basisDecision.MilestoneList = new List<DecisionMilestone>();
            if (_basisDecision != null && !string.IsNullOrEmpty(_basisDecision.LinkId))
            {
                var paatList = dbConnection.GetPostAuthActivityListById(id);
                _basisDecision.DinList = dbConnection.GetBasicDecisionDinListById(id);
                if (paatList != null && paatList.Count > 0)
                {
                    _basisDecision.PostActivityList = paatList;                   
                }
                //Milestones.
                var mileList = dbConnection.GetDecisionMilestoneListById(id);
                if (mileList != null && mileList.Count > 0)
                {
                    _basisDecision.MilestoneList = mileList;
                }
            }
            return _basisDecision;
        }

        public BasisDecisionMedicalDevice GetMedicalDevice(string lang, string id)
        {
            DBConnection dbConnection = new DBConnection(lang);
            _basisDecisionMd = dbConnection.GetBasisDecisionMedicalDeviceById(id);
            _basisDecisionMd.PlatList = new List<PostLicensingActivity>();
            _basisDecisionMd.AppMilestoneList = new List<ApplicationMilestones>();
            if (_basisDecisionMd != null && !string.IsNullOrEmpty(_basisDecisionMd.LinkId))
            {
                var platList = dbConnection.GetPostLicensingActivityListById(id);
                if (platList != null && platList.Count > 0)
                {
                    _basisDecisionMd.PlatList = platList;
                }
                //Milestones.
                var mileList = dbConnection.GetApplicationMilestoneListById(id);
                if (mileList != null && mileList.Count > 0)
                {
                    _basisDecisionMd.AppMilestoneList = mileList;
                }
            }
            return _basisDecisionMd;
        }
    }
}