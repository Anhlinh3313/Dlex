using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data.Abstract;
using Core.Entity.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Data.Core
{
    public class EntityVPRepository<T> : IEntityVPRepository<T>
        where T : class, IEntityProcView, new()
    {
        private ApplicationContext _context;
        private readonly IConfiguration congiguration;
        public EntityVPRepository(ApplicationContext context)
        {
            _context = context;
        }

        public T ExecProcedureSingle(IEntityProc entityProc)
        {
                return _context.Set<T>().FromSql(entityProc.GetQuery(), entityProc.GetParams()).AsEnumerable().FirstOrDefault();
        }

        public T ExecProcedureSingle(string query, params object[] parameters)
        {
                return _context.Set<T>().FromSql(query, parameters).AsEnumerable().FirstOrDefault();
        }

        public IEnumerable<T> ExecProcedure(IEntityProc entityProc)
        {
                return _context.Set<T>().FromSql(entityProc.GetQuery(), entityProc.GetParams());
        }

        public IEnumerable<T> ExecProcedure(string query, params object[] parameters)
        {
                return _context.Set<T>().FromSql(query, parameters);
        }


        public async Task<IEnumerable<T>> GetReportListShipmentExport(IEntityProc entityProc)
        {
            _context.Database.OpenConnection();
            var data = await _context.Set<T>().FromSql(entityProc.GetQuery(), entityProc.GetParams()).ToListAsync();

            _context.Database.CloseConnection();
            return data;
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
