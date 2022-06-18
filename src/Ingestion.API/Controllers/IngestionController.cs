using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using Ingestion.API.Model;
using Ingestion.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ingestion.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngestionController : ControllerBase
    {
        private readonly IFileDirectory _fileDirectory;
        private readonly ILogger<IngestionController> _logger;

        public IngestionController(ILogger<IngestionController> logger, 
            IFileDirectory fileDirectory)
        {
            _logger = logger;
            _fileDirectory = fileDirectory;
        }
        
        [HttpPost]
        public IActionResult GenerateFile([FromQuery] int numRows = 100)
        {
            var fileName = Guid.NewGuid().ToString();

            var dataPath = _fileDirectory.GetDataDirectory();
            using var writer = new StreamWriter(Path.Join(dataPath, fileName));
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteHeader<Person>();
            csv.NextRecord();
            for (var i = 0; i < numRows; i++)
            {
                csv.WriteRecord(PersonFactory.GetGenericPerson());
                csv.NextRecord();
            }

            return Ok(fileName);
        }

        [HttpGet] public IActionResult AvailableDataFiles()
        {
            var dataPath = _fileDirectory.GetDataDirectory();

            var files = Directory.EnumerateFiles(dataPath);

            return Ok(new 
            {
                dataPath,
                files
            });
        }
    }
}