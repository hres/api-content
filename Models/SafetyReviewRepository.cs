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
            return _safetyReviews;
        }

        public SafetyReview Get(string lang, string id)
        {
            DBConnection dbConnection = new DBConnection(lang);
            _safetyReview = dbConnection.GetSafetyReviewById(id);
            if(_safetyReview != null && !string.IsNullOrEmpty(_safetyReview.LinkId) )
            {

                var bulletList = dbConnection.GetSafetyReviewBulletListById(id);
                if(bulletList != null && bulletList.Count > 0)
                {
                    _safetyReview.KeyMessageList = new List<BullePoint>();
                    _safetyReview.FootnotesList = new List<BullePoint>();
                    _safetyReview.UseCanadaList = new List<BullePoint>();
                    _safetyReview.FindingList = new List<BullePoint>();
                    _safetyReview.ConclusionList = new List<BullePoint>();
                    _safetyReview.ReferenceList = new List<BullePoint>();

                    //KeyMessage
                    _safetyReview.KeyMessageList = bulletList.Where(x => x.FieldId == _safetyReview.KeyMessages).OrderBy(x => x.OrderNo).ToList();
                    //footnotes
                    _safetyReview.FootnotesList = bulletList.Where(x => x.FieldId == _safetyReview.Footnotes).OrderBy(x => x.OrderNo).ToList();
                    //UseCanada
                    _safetyReview.UseCanadaList = bulletList.Where(x => x.FieldId == _safetyReview.UseCanada).OrderBy(x => x.OrderNo).ToList();
                    //Findings
                    _safetyReview.FindingList = bulletList.Where(x => x.FieldId == _safetyReview.Findings).OrderBy(x => x.OrderNo).ToList();
                    //Conclusion
                    _safetyReview.ConclusionList = bulletList.Where(x => x.FieldId == _safetyReview.Conclusion).OrderBy(x => x.OrderNo).ToList();
                    //Reference
                    _safetyReview.ReferenceList = bulletList.Where(x => x.FieldId == _safetyReview.References).OrderBy(x => x.OrderNo).ToList();
                }
            }

            return _safetyReview;
        }
    }
}