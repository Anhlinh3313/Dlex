using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class EntityProc : IEntityProc
    {
        private readonly string _query;
        private readonly SqlParameter[] _pars;
        private readonly string _procname;

        public EntityProc(string query, SqlParameter[] pars,string procname = null)
        {
            _query = query;
            _pars = pars;
            _procname = procname;
        }

        public SqlParameter[] GetParams()
        {
            return _pars;
        }

        public string GetQuery()
        {
            return _query;
        }
        public string GetProcName()
        {
            return _procname;
        }
    }
}
