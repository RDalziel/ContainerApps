using System;
using System.IO;
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
        public IActionResult GenerateFile()
        {
            var fileName = Guid.NewGuid().ToString();

            var dataPath = _fileDirectory.GetDataDirectory();
            System.IO.File.Create(Path.Join(dataPath, fileName));

            return Ok(fileName);
        }
    }
}