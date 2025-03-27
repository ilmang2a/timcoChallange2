using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using timcoChallange2.Models;
using System.Text;
using System.IO;
using System.Text.Json;

namespace CSVHelper
{
    public class CSVHelper
    {
        private readonly ILogger<CSVHelper> _logger;



        public CSVHelper(ILogger<CSVHelper> logger)
        {

            _logger = logger;


        }

        [Function("CSVHelper")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "Post")] HttpRequestData req)
        {
            var data = await JsonSerializer.DeserializeAsync<Person>(req.Body);
            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            return response;
        }
    }
}
