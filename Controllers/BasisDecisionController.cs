using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using regContentWebApi.Models;

namespace regContentWebApi.Controllers
{
    public class BasisDecisionController : ApiController
    {
        static readonly IBasisDecisionRepository databasePlaceholder = new BasisDecisionRepository();

        public IEnumerable<BasisDecision> GetAllBasisDecision(string lang="en")
        {

            return databasePlaceholder.GetAll(lang);
        }


        public BasisDecision GetBasisDecisionByID(string id, string lang = "en")
        {

            BasisDecision basisDecision = databasePlaceholder.Get(lang,id);
            if (basisDecision == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return basisDecision;
        }
    }
}