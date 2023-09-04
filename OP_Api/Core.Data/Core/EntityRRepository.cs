using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Data.Abstract;
using Core.Entity.Abstract;
using Core.Infrastructure.Http;
using Core.Infrastructure.Utils;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Core
{
    public class EntityRRepository<T> : IEntityRRepository<T>
        where T : class, IEntityBase, new()
    {
        private ApplicationContext _context;
        private int _companyId;
        public EntityRRepository(ApplicationContext context)
        {
            _context = context;
            if (HttpContext.Current != null)
            {
                _companyId = HttpContext.CurrentCompanyId;
            }
        }

        public bool Any()
        {
            return _context.Set<T>().Any<T>(f50P_R26 => f50P_R26.IsEnabled == true && f50P_R26.CompanyId == _companyId);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            predicate = predicate.And(f50P_R31 => f50P_R31.IsEnabled == true && f50P_R31.CompanyId == _companyId);
            return _context.Set<T>().Any<T>(predicate);
        }

        public Task<bool> AnyAsync()
        {
            return _context.Set<T>().AnyAsync<T>(f50P_R36 => f50P_R36.IsEnabled == true && f50P_R36.CompanyId == _companyId);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            predicate = predicate.And(f50P_R41 => f50P_R41.IsEnabled == true && f50P_R41.CompanyId == _companyId);
            return _context.Set<T>().AnyAsync<T>(predicate);
        }

        public int Count()
        {
            return _context.Set<T>().Count(f50P_R46 => f50P_R46.IsEnabled == true && f50P_R46.CompanyId == _companyId);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            predicate = predicate.And(f50P_R51 => f50P_R51.IsEnabled == true && f50P_R51.CompanyId == _companyId);
            return _context.Set<T>().Count(predicate);
        }

        public Task<int> CountAsync()
        {
            return _context.Set<T>().CountAsync(f50P_R56 => f50P_R56.IsEnabled == true && f50P_R56.CompanyId == _companyId);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            predicate = predicate.And(f50P_R61 => f50P_R61.IsEnabled == true && f50P_R61.CompanyId == _companyId);
            return _context.Set<T>().CountAsync(predicate);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            predicate = predicate.And(f50P_R66 => f50P_R66.IsEnabled == true && f50P_R66.CompanyId == _companyId);
            return _context.Set<T>().Where(predicate);
        }

        public IAsyncEnumerable<T> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            predicate = predicate.And(f50P_R71 => f50P_R71.IsEnabled == true && f50P_R71.CompanyId == _companyId);
            return _context.Set<T>().Where(predicate).ToAsyncEnumerable();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, string[] includeProperties)
        {
            T t = new T();
            List<string> listIncludeProps = new List<string>();

            foreach (var item in includeProperties)
            {
                if (ClassUtil.HasProperty(t, item))
                {
                    listIncludeProps.Add(item);
                }
            }
            predicate = predicate.And(f50P_R81 => f50P_R81.IsEnabled == true && f50P_R81.CompanyId == _companyId);
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            foreach (var includeProperty in listIncludeProps)
            {
                try
                {
                    query = query.Include(includeProperty);
                }
                catch { }
            }
            return query;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            predicate = predicate.And(f50P_R102 => f50P_R102.IsEnabled == true && f50P_R102.CompanyId == _companyId);
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IAsyncEnumerable<T> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            predicate = predicate.And(f50P_R113 => f50P_R113.IsEnabled == true && f50P_R113.CompanyId == _companyId);
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.ToAsyncEnumerable();
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().Where(f50134 => f50134.IsEnabled == true && f50134.CompanyId == _companyId);
        }

        public IQueryable<T> GetAll(string[] includeProperties)
        {
            T t = new T();
            List<string> listIncludeProps = new List<string>();

            foreach (var item in includeProperties)
            {
                if (ClassUtil.HasProperty(t, item))
                {
                    listIncludeProps.Add(item);
                }
            }
            IQueryable<T> query = _context.Set<T>().Where(f50P_R139 => f50P_R139.IsEnabled == true && f50P_R139.CompanyId == _companyId);
            foreach (var includeProperty in listIncludeProps)
            {
                try
                {
                    query = query.Include(includeProperty);
                }
                catch { }
            }
            return query;
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().Where(f50P_R153 => f50P_R153.IsEnabled == true && f50P_R153.CompanyId == _companyId);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IAsyncEnumerable<T> GetAllAsync()
        {
            return _context.Set<T>().Where(f50P_R163 => f50P_R163.IsEnabled == true && f50P_R163.CompanyId == _companyId).ToAsyncEnumerable();
        }

        public IAsyncEnumerable<T> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().Where(f50P_R168 => f50P_R168.IsEnabled == true && f50P_R168.CompanyId == _companyId);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.ToAsyncEnumerable();
        }


        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, string[] includeProperties)
        {
            T t = new T();
            List<string> listIncludeProps = new List<string>();

            foreach (var item in includeProperties)
            {
                if (ClassUtil.HasProperty(t, item))
                {
                    listIncludeProps.Add(item);
                }
            }
            predicate = predicate.And(f50P_R189 => f50P_R189.IsEnabled == true && f50P_R189.CompanyId == _companyId);
            IQueryable<T> query = _context.Set<T>().Where(predicate);

            foreach (var includeProperty in listIncludeProps)
            {
                try
                {
                    query = query.Include(includeProperty);
                }
                catch { }
            }
            return query;
        }

        public T GetSingle(int id)
        {
            return _context.Set<T>().FirstOrDefault(f50P_R204 => f50P_R204.Id == id && f50P_R204.IsEnabled == true && f50P_R204.CompanyId == _companyId);
        }

        public T GetSingle(int id, string[] includeProperties)
        {
            T t = new T();
            List<string> listIncludeProps = new List<string>();

            foreach (var item in includeProperties)
            {
                if (ClassUtil.HasProperty(t, item))
                {
                    listIncludeProps.Add(item);
                }
            }
            IQueryable<T> query = _context.Set<T>().Where(f50P_R219 => f50P_R219.Id == id && f50P_R219.IsEnabled == true && f50P_R219.CompanyId == _companyId);
            foreach (var includeProperty in listIncludeProps)
            {
                try
                {
                    query = query.Include(includeProperty);
                }
                catch { }
            }
            return query.FirstOrDefault();
        }

        public T GetSingle(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().Where(f50P_R233 => f50P_R233.Id == id && f50P_R233.IsEnabled == true && f50P_R233.CompanyId == _companyId);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefault();
        }

        public Task<T> GetSingleAsync(int id)
        {
            return _context.Set<T>().FirstOrDefaultAsync(f50P_R243 => f50P_R243.Id == id && f50P_R243.IsEnabled == true && f50P_R243.CompanyId == _companyId);
        }

        public Task<T> GetSingleAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().Where(f50_R254 => f50_R254.Id == id && f50_R254.IsEnabled == true && f50_R254.CompanyId == _companyId);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefaultAsync();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            predicate = predicate.And(f50P_R248 => f50P_R248.IsEnabled == true && f50P_R248.CompanyId == _companyId);
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            predicate = predicate.And(f50P_R264 => f50P_R264.IsEnabled == true && f50P_R264.CompanyId == _companyId);
            return _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate, string[] includeProperties)
        {
            T t = new T();
            List<string> listIncludeProps = new List<string>();
            foreach (var item in includeProperties)
            {
                if (ClassUtil.HasProperty(t, item))
                {
                    listIncludeProps.Add(item);
                }
            }
            predicate = predicate.And(f50P_R280 => f50P_R280.IsEnabled == true && f50P_R280.CompanyId == _companyId);
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            foreach (var includeProperty in listIncludeProps)
            {
                try
                {
                    query = query.Include(includeProperty);
                }
                catch { }
            }
            return query.FirstOrDefault();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            predicate = predicate.And(f50P_R295 => f50P_R295.IsEnabled == true && f50P_R295.CompanyId == _companyId);
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefault();
        }

        public Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            predicate = predicate.And(f50P_R306 => f50P_R306.IsEnabled == true && f50P_R306.CompanyId == _companyId);
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefaultAsync();
        }

        #region IDisposable Support
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
