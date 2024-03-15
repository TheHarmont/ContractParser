using Microsoft.Extensions.FileProviders;
using System.Security.Cryptography;

namespace ContractParser.Models
{
    public class FileInfo
    {
        public string fileName { get; }
        public string fileHash { get; }
        public string filePath { get; }
        public long fileSize { get; }

        public FileInfo(string path)
        {
            var file = new System.IO.FileInfo(path);
            fileName = file.Name;
            fileHash = CalculateFileHash(path);
            filePath = path;
            fileSize = file.Length;
        }

        /// <summary>
        /// Возвращает хэш содержимого файла
        /// </summary>
        private string CalculateFileHash(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }
    }
}
