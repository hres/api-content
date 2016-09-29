using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regContentWebApi.Models
{
    interface ISafetyReviewRepository
    {
        IEnumerable<SafetyReview> GetAll(string lang);
        SafetyReview Get(string lang, string id);
    }
}
