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

namespace CSVHelper
{
    public class CSVHelper
    {
        private readonly ILogger<CSVHelper> _logger;
        private readonly List<Person> people;



        public CSVHelper(ILogger<CSVHelper> logger)
        {
            people = new List<Person>();

            _logger = logger;
            people.Add(new Person { Name = "Ilman", Surename = "Hamzatov", Email = "ilman@mail.de" });
            people.Add(new Person { Name = "Donald", Surename = "Duck", Email = "donald@mail.de" });
            people.Add(new Person { Name = "Trump", Surename = "Donald", Email = "trump@mail.de" });
            people.Add(new Person { Name = "Che", Surename = "Guevera", Email = "che@mail.de" });

        }

        [Function("CSVHelper")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {


            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/csv");
            response.Headers.Add("Content-Disposition", "attachment; filename=\"people.csv\"");


            var memoryStream = new MemoryStream();


            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            try
            {
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, leaveOpen: true))

                using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(people);
                    await streamWriter.FlushAsync();

                }
                memoryStream.Position = 0;


            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to write CSV file: {ex.Message}");
                throw;
            }


            await memoryStream.CopyToAsync(response.Body);
            return response;
        }
    }
}
