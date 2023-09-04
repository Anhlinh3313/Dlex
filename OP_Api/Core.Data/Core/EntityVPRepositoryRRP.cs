using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using System.Threading.Tasks;
using Core.Data.Abstract;
using Core.Entity.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using Core.Api;

namespace Core.Data.Core
{
    public class EntityVPRepositoryRRP<T> : IEntityVPRepository<T>
        where T : class, IEntityProcView, new()
    {
        private readonly IConfiguration congiguration;
        private ApplicationContextRRP _contextRRP;


        //private ApplicationContextRRP _contextRRP;
        public EntityVPRepositoryRRP(ApplicationContextRRP contextRRP)
        {
            _contextRRP = contextRRP;
            //hhhh
        }

        public T ExecProcedureSingle(IEntityProc entityProc)
        {
                return _contextRRP.Set<T>().FromSql(entityProc.GetQuery(), entityProc.GetParams()).AsEnumerable().FirstOrDefault();
        }

        public T ExecProcedureSingle(string query, params object[] parameters)
        {
                return _contextRRP.Set<T>().FromSql(query, parameters).AsEnumerable().FirstOrDefault();
        }

        public IEnumerable<T> ExecProcedure(IEntityProc entityProc)
        {
            _contextRRP.Database.OpenConnection();
                var data =  _contextRRP.Set<T>().FromSql(entityProc.GetQuery(), entityProc.GetParams());

            _contextRRP.Database.CloseConnection();
            return data;
        }

        public IEnumerable<T> ExecProcedure(string query, params object[] parameters)
        {
                return _contextRRP.Set<T>().FromSql(query, parameters);
        }

        public async Task<IEnumerable<T>> GetReportListShipmentExport(IEntityProc entityProc)
        {
            _contextRRP.Database.OpenConnection();
            var data = await _contextRRP.Set<T>().FromSql(entityProc.GetQuery(), entityProc.GetParams()).ToListAsync();

            _contextRRP.Database.CloseConnection();
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
                    _contextRRP.Dispose();
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
