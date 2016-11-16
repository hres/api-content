using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regContentWebApi.Models
{
    interface IRegulatoryDecisionRepository
    {
        IEnumerable<RegulatoryDecision> GetAll(string lang);
        RegulatoryDecision Get(string lang, string id);

        IEnumerable<RegulatoryDecisionMedicalDevice> GetAllMedicalDevice(string lang);
        RegulatoryDecisionMedicalDevice GetMedicalDevice(string lang, string id);
    }
}
