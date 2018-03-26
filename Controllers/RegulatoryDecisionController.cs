using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using regContentWebApi.Models;

namespace regContentWebApi.Controllers
{
    public class RegulatoryDecisionController : ApiController
    {
        static readonly IRegulatoryDecisionRepository databasePlaceholder = new RegulatoryDecisionRepository();

        public IEnumerable<RegulatoryDecision> GetAllRegulatoryDecision(string lang="en")
        {

            return databasePlaceholder.GetAll(lang);
        }
       
        public RegulatoryDecision GetRegulatoryDecisionByID(string id, string lang="en")
        {
            var regulatoryDecision = databasePlaceholder.Get(lang, id);
         
            if (regulatoryDecision == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return regulatoryDecision;
        }        
    }
}