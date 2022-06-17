using System.IO;
using System.Runtime.Serialization.Formatters;

namespace Ingestion.API.Services
{
    public interface IFileDirectory
    {
        string GetDataDirectory();
    }

    public class FileDirectory : IFileDirectory
    {
        private readonly string _rootDataFileDirectory;

        public FileDirectory(string rootDataFileDirectory)
        {
            _rootDataFileDirectory = rootDataFileDirectory;

            if (!Directory.Exists(rootDataFileDirectory))
            {
                Directory.CreateDirectory(rootDataFileDirectory);
            }
        }

        public string GetDataDirectory()
        {
            return _rootDataFileDirectory;
        }
    }

}