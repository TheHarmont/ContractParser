using ContractParser.Domain.Abstract;
using ContractParser.Domain.DAL;
using ContractParser.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ContractParser.Services
{
    public class ContractParserService
    {
        private readonly IContractRepository _contractRepository;
        private readonly ILogger<ContractParserService> _logger;

        public ContractParserService(ILogger<ContractParserService> logger, IContractRepository contractRepository)
        {
            _logger = logger;
            _contractRepository = contractRepository;
        }

        public void Application()
        {
            //Получение основных директорий
            string varPath = Path.Combine(Directory.GetCurrentDirectory(), "var");
            string filesPath = Path.Combine(varPath, "files");
            string historyPath = Path.Combine(varPath, "history");
            string errorPath = Path.Combine(varPath, "error");
            string searchPattern = "*.xml";

            var files = DirectoryHandler.GetFilesFromDirectory(filesPath, searchPattern);

            //Если файлов нет, то останавливаем работу
            if (files.Count() == 0) return;

            _logger.LogInformation($"Найдено файлов: {files.Count()}. Производится загрузка данных.");

            foreach (var file in files)
            {
                try
                {
                    //Проверка на наличие такого файла в БД
                    if (_contractRepository.IsExist(file.fileHash))
                    {
                        throw new Exception("Похожий файл найден в базе данных.");
                    }

                    //Основной метод получения данных
                    var correctFileData = XMLHandler.GetFileData(file);

                    _contractRepository.AddContracts(correctFileData);

                    //Пеносим успешный файл в папку "history" с НОВЫМ названием
                    File.Move(file.filePath, Path.Combine(historyPath, DirectoryHandler.GenerateNewFileName(file.fileName)));
                    _logger.LogInformation($"Файл {Path.GetFileName(file.filePath)} успешно загружен.");
                }
                catch (Exception ex)
                {
                    //Переносим ошибочный файл в папку "error" со СТАРЫМ названием
                    File.Move(file.filePath, Path.Combine(errorPath, file.fileName));
                    _logger.LogError($"Ошибка в файле: {Path.GetFileName(file.filePath)}. Текст ошибки: {ex.Message}.");
                }
            }
        }
    }
}
