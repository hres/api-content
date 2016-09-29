using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regContentWebApi.Models
{
    interface IBasisDecisionRepository
    {
        IEnumerable<BasisDecision> GetAll(string lang);
        BasisDecision Get(string lang, string id);

        BasisDecisionMedicalDevice GetMedicalDevice(string lang, string id);
    }
}
