using regContentWebApi;
using System.Collections.Generic;
using System.Linq;

namespace regContentWebApi.Models
{
    public class SafetyReviewRepository : ISafetyReviewRepository
    {
        private List<SafetyReview> _safetyReviews = new List<SafetyReview>();
        private SafetyReview _safetyReview = new SafetyReview();
        

        public IEnumerable<SafetyReview> GetAll(string lang)
        {
            DBConnection dbConnection = new DBConnection(lang);
            _safetyReviews = dbConnection.GetAllSafetyReview();
            if (_safetyReviews!=null && _safetyReviews.Count > 0)
            {
                foreach(var _safetyReview in _safetyReviews)
                {
                    if (_safetyReview != null && !string.IsNullOrEmpty(_safetyReview.link_id))
                    {

                        var bulletList = dbConnection.GetSafetyReviewBulletListById(_safetyReview.link_id);
                        if (bulletList != null && bulletList.Count > 0)
                        {
                            _safetyReview.key_message_list = new List<BullePoint>();
                            _safetyReview.footnotes_list = new List<BullePoint>();
                            _safetyReview.use_canada_list = new List<BullePoint>();
                            _safetyReview.finding_list = new List<BullePoint>();
                            _safetyReview.conclusion_list = new List<BullePoint>();
                            _safetyReview.reference_list = new List<BullePoint>();

                            //KeyMessage
                            _safetyReview.key_message_list = bulletList.Where(x => x.field_id == _safetyReview.key_messages).OrderBy(x => x.order_no).ToList();
                            //footnotes
                            _safetyReview.footnotes_list = bulletList.Where(x => x.field_id == _safetyReview.footnotes).OrderBy(x => x.order_no).ToList();
                            //UseCanada
                            _safetyReview.use_canada_list = bulletList.Where(x => x.field_id == _safetyReview.use_canada).OrderBy(x => x.order_no).ToList();
                            //Findings
                            _safetyReview.finding_list = bulletList.Where(x => x.field_id == _safetyReview.findings).OrderBy(x => x.order_no).ToList();
                            //Conclusion
                            _safetyReview.conclusion_list = bulletList.Where(x => x.field_id == _safetyReview.conclusion).OrderBy(x => x.order_no).ToList();
                            //Reference
                            _safetyReview.reference_list = bulletList.Where(x => x.field_id == _safetyReview.references).OrderBy(x => x.order_no).ToList();
                        }
                    }
                }
            }

            return _safetyReviews;
        }

        public SafetyReview Get(string lang, string id)
        {
            DBConnection dbConnection = new DBConnection(lang);
            _safetyReview = dbConnection.GetSafetyReviewById(id);
            if(_safetyReview != null && !string.IsNullOrEmpty(_safetyReview.link_id) )
            {

                var bulletList = dbConnection.GetSafetyReviewBulletListById(id);
                if(bulletList != null && bulletList.Count > 0)
                {
                    _safetyReview.key_message_list = new List<BullePoint>();
                    _safetyReview.footnotes_list = new List<BullePoint>();
                    _safetyReview.use_canada_list = new List<BullePoint>();
                    _safetyReview.finding_list = new List<BullePoint>();
                    _safetyReview.conclusion_list = new List<BullePoint>();
                    _safetyReview.reference_list = new List<BullePoint>();

                    //KeyMessage
                    _safetyReview.key_message_list = bulletList.Where(x => x.field_id == _safetyReview.key_messages).OrderBy(x => x.order_no).ToList();
                    //footnotes
                    _safetyReview.footnotes_list = bulletList.Where(x => x.field_id == _safetyReview.footnotes).OrderBy(x => x.order_no).ToList();
                    //UseCanada
                    _safetyReview.use_canada_list = bulletList.Where(x => x.field_id == _safetyReview.use_canada).OrderBy(x => x.order_no).ToList();
                    //Findings
                    _safetyReview.finding_list = bulletList.Where(x => x.field_id == _safetyReview.findings).OrderBy(x => x.order_no).ToList();
                    //Conclusion
                    _safetyReview.conclusion_list = bulletList.Where(x => x.field_id == _safetyReview.conclusion).OrderBy(x => x.order_no).ToList();
                    //Reference
                    _safetyReview.reference_list = bulletList.Where(x => x.field_id == _safetyReview.references).OrderBy(x => x.order_no).ToList();
                }
            }

            return _safetyReview;
        }
    }
}