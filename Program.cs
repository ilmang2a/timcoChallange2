using CSVHelper;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
  .ConfigureFunctionsWebApplication()
  .ConfigureAppConfiguration(c =>
  {
      c.AddJsonFile("appsettings.json", optional: true);
      c.AddJsonFile("local.settings.json", optional: true);
      c.AddEnvironmentVariables();
  }).Build();

host.Run();


