using CsvHelper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Globalization;
using timcoChallange2.Models;
using System.Text.Json;
using System.Net;
using Microsoft.Extensions.Configuration;
using CsvHelper.Configuration;

namespace CSVHelper
{
    public class CSVHelperExport
    {
        private readonly IConfiguration _config;

        public CSVHelperExport(IConfiguration configuration)
        {
            _config = configuration;
        }

        [Function("CSVHelperExport")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            var persons = await JsonSerializer.DeserializeAsync<List<Person>>(req.Body);
            if (persons == null || persons.Count == 0)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            bool? includeHeader = _config.GetValue<bool?>("IncludeHeader");
            if (includeHeader == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            using Stream stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = (bool)includeHeader,
            });

            csv.WriteRecords(persons);
            csv.Flush();
            stream.Position = 0;

            var output = req.CreateResponse(HttpStatusCode.OK);
            output.Headers.Add("Content-Type", "text/csv");
            output.Headers.Add("Content-Disposition", "attachment; filename=persons.csv");
            await stream.CopyToAsync(output.Body);
            return output;

        }
    }
}
