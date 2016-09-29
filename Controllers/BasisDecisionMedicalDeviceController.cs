using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using regContentWebApi.Models;

namespace regContentWebApi.Controllers
{
    public class BasisDecisionMedicalDeviceController : ApiController
    {
        static readonly IBasisDecisionRepository databasePlaceholder = new BasisDecisionRepository();

        public IEnumerable<BasisDecision> GetAllBasisDecision(string lang)
        {

            return databasePlaceholder.GetAll(lang);
        }


        public BasisDecisionMedicalDevice GetMDById(string lang, string id)
        {
            BasisDecisionMedicalDevice basisDecisionMd = databasePlaceholder.GetMedicalDevice(lang,id);
            if (basisDecisionMd == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return basisDecisionMd;
        }

        
    }
}