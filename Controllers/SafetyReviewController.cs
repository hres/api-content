﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using regContentWebApi.Models;

namespace regContentWebApi.Controllers
{
    public class SafetyReviewController : ApiController
    {
        static readonly ISafetyReviewRepository databasePlaceholder = new SafetyReviewRepository();

        public IEnumerable<SafetyReview> GetAllSafetyReview(string lang="en")
        {

            return databasePlaceholder.GetAll(lang);
        }


        public SafetyReview GetSafetyReviewByID(string id, string lang="en")
        {
            SafetyReview safetyReview = databasePlaceholder.Get(lang, id);
            if (safetyReview == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return safetyReview;
        }
    }
}