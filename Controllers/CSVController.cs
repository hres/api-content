using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using System.Net.Http.Headers;
using System.Text;


namespace regContentWebApi.Controllers
{
    public class CSVController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage DownloadCSV(string dataType)
        {
            DBConnection dbConnection = new DBConnection("en");
            var jsonResult = string.Empty;
            var fileNameDate = string.Format("{0}{1}{2}",
                           DateTime.Now.Year.ToString(),
                           DateTime.Now.Month.ToString().PadLeft(2, '0'),
                           DateTime.Now.Day.ToString().PadLeft(2, '0'));
            var fileName = string.Format(dataType + "_{0}.csv", fileNameDate);
            byte[] outputBuffer = null;
            string resultString = string.Empty;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            var json = string.Empty;
            
            switch (dataType)
            {
                case "basisDecision":
                    var basisDecisions = dbConnection.GetAllBasisDecision().ToList();
                    if (basisDecisions.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(basisDecisions);

                    }
                    break;

                case "basisDecisionMedical":
                    var basisDecisionMedical = dbConnection.GetAllBasisDecisionMedicalDevice().ToList();
                    if (basisDecisionMedical.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(basisDecisionMedical);

                    }
                    break;

                case "regDecisionMedical":
                    var regDecisionMedical = dbConnection.GetAllRegulatoryDecisionMedicalDevices().ToList();
                    if (regDecisionMedical.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(regDecisionMedical);
                    }
                    break;

                case "regDecision":
                    var regDecisions = dbConnection.GetAllRegulatoryDecision().ToList();
                    if (regDecisions.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(regDecisions);

                    }
                    break;

                case "safetyReview":
                    var safetyReviews = dbConnection.GetAllSafetyReview().ToList();
                    if (safetyReviews.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(safetyReviews);

                    }
                    break;
            }
            
            if (!string.IsNullOrWhiteSpace(json))
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                if (dt.Rows.Count > 0)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            UtilityHelper.WriteDataTable(dt, writer, true);
                            outputBuffer = stream.ToArray();
                            resultString = Encoding.UTF8.GetString(outputBuffer, 0, outputBuffer.Length);
                        }
                    }
                    result.Content = new StringContent(resultString);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName };
                }
            }

            return result;
        }
    }
}