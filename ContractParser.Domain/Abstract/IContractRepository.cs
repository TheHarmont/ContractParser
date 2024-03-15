using ContractParser.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractParser.Domain.Abstract
{
    public interface IContractRepository
    {
        /// <summary>
        /// Пакетный импорт данных в БД
        /// </summary>
        public bool AddContracts(List<Entity.Contract> contracts);

        /// <summary>
        /// Проверить на наличие такого файла в БД
        /// </summary>
        public bool IsExist(string fileHash);
    }
}
