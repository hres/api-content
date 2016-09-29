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
                    item.DinList = new List<string>(); //To get din list
                    item.DinList = dbConnection.GetRegulatoryDinListById(item.LinkId);    
               }                
            }
            
            return _regDecisions;
        }

        public RegulatoryDecision Get(string lang, string id)
        {
            DBConnection dbConnection = new DBConnection(lang);
            _regDecision = dbConnection.GetRegulatoryDecisionById(id);
            if (_regDecision != null && !string.IsNullOrEmpty(_regDecision.LinkId))
            {
                    
                _regDecision.DinList = dbConnection.GetRegulatoryDinListById(id);
                _regDecision.BulletList = new List<BullePoint>();
                var bulletList = dbConnection.GetRegulatoryDecisioBulletListById(id);
                if (bulletList != null && bulletList.Count > 0)
                {
                    //footnotes ( will be uncommented once field_id added in the db)
                    _regDecision.BulletList = bulletList.OrderBy(c => c.OrderNo).ToList();
                }
            }
            return _regDecision;
        }
    }
}