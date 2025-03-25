using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using timcoChallange2.Models;

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



            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            try
            {
                string filePath = "C:\\Users\\admhamzatov\\source\\repos\\timcoChallange2\\CSV\\output.csv";

                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(people);
                }

                _logger.LogInformation($"CSV file successfully written to {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to write CSV file: {ex.Message}");
                throw;
            }


            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(people);

            return response;
        }
    }
}
