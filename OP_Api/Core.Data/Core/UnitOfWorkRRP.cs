using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Infrastructure.Http;
using Core.Data.Abstract;
using Core.Entity.Abstract;
using Core.Entity.Entities;
using System.Threading.Tasks;

namespace Core.Data.Core
{
    public class UnitOfWorkRRP : BaseDisposable, IUnitOfWorkRRP
    {
        private readonly ApplicationContextRRP _contextRRP;
        private Dictionary<string, object> _repository;

        public UnitOfWorkRRP(ApplicationContextRRP contextRRP = null)
        {
            _contextRRP = contextRRP;
            _repository = new Dictionary<string, object>();
        }

        public IEntityVPRepository<T> Repository<T>() where T : class, IEntityProcView, new()
        {
            if (_repository == null)
            {
                _repository = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!_repository.ContainsKey(type))
            {
                    var repositoryType = typeof(EntityVPRepositoryRRP<>);
                    var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _contextRRP);
                    _repository.Add(type, repositoryInstance);
            }
            return (EntityVPRepositoryRRP<T>)_repository[type];
        }
        public int Commit()
        {
            return _contextRRP.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return _contextRRP.SaveChangesAsync();
        }

        public void Rollback()
        {
            _contextRRP.ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }

        protected override void DisposeCore()
        {
            if (_contextRRP != null)
                _contextRRP.Dispose();
        }
    }
}
