using ContractParser.Domain.Abstract;
using FileInfo = ContractParser.Models.FileInfo;

namespace ContractParser.Handlers
{
    public static class DirectoryHandler
    {
        /// <summary>
        /// Создает новое имя для файла, сохраняя путь.
        /// Пример: export_20240305142323211.xml
        /// </summary>
        public static string GenerateNewFileName(string fileName)
        {
            return $"export_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{Path.GetExtension(fileName)}";
        }

        /// <summary>
        /// Возвращает список с информацией о файлах в директории.
        /// </summary>
        public static List<FileInfo> GetFilesFromDirectory(string filePath, string searchPattern)
        {
            return Directory.GetFiles(filePath, searchPattern)
                    .Select(filePath => new FileInfo(filePath))
                    .ToList();
        }
    }
}
