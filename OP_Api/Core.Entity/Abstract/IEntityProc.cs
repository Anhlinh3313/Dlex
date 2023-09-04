using System;
using System.Data.SqlClient;

namespace Core.Entity.Abstract
{
    public interface IEntityProc : IEntityProcView
    {
		SqlParameter[] GetParams();
		string GetQuery();
        string GetProcName();
    }
}
