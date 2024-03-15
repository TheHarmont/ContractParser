using ContractParser.Domain.Abstract;
using ContractParser.Domain.Entity;
using System.Diagnostics.Contracts;

namespace ContractParser.Domain.DAL
{
    public class EFContractRepository : IContractRepository
    {
        private readonly DataBaseContext _db;

        public EFContractRepository(DataBaseContext db)
        {
            _db = db;
        }

        public bool AddContracts(List<Entity.Contract> contracts)
        {
            try
            {
                _db.Contracts.AddRange(contracts);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool IsExist(string fileHash)
        {
            return _db.Contracts.FirstOrDefault(x => x.sourceHash == fileHash) != null;
        }
    }
}
