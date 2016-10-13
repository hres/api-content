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
    }
}