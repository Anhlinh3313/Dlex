using System;
using System.Threading.Tasks;
using Core.Entity.Abstract;

namespace Core.Data.Abstract
{
    public interface IUnitOfWorkRRP : IDisposable
    {
        IEntityVPRepository<T> Repository<T>() where T : class, IEntityProcView, new();
		int Commit();
		Task<int> CommitAsync();
        void Rollback();
    }
}
