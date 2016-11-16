﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using regContentWebApi.Models;

namespace regContentWebApi.Controllers
{
    public class RegulatoryDecisionMedicalDeviceController : ApiController
    {
        static readonly IRegulatoryDecisionRepository databasePlaceholder = new RegulatoryDecisionRepository();

        public IEnumerable<RegulatoryDecisionMedicalDevice> GetAllRegulatoryDecisionMedicalDevice(string lang)
        {

            return databasePlaceholder.GetAllMedicalDevice(lang);
        }

        public RegulatoryDecisionMedicalDevice GetRegulatoryDecisionMedicalDeviceByID(string lang, string id)
        {
            var regulatoryDecisionMD = databasePlaceholder.GetMedicalDevice(lang, id);

            if (regulatoryDecisionMD == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return regulatoryDecisionMD;
        }

    }
}